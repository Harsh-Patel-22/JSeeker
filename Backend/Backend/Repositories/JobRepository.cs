using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class JobRepository (ApplicationDbContext context) {
    
    public async Task<JobStatus> GetJobStatusAsync(int jobId) {
        JobStatus status = await context.Jobs.Where(j => j.Id == jobId).Select(j => j.Status).FirstOrDefaultAsync();
        return status;
    }
    
    public async Task<int> GetJobApplicationLimitAsync(int jobId) {
        int jobApplicationLimit = await context.Jobs.Where(j => j.Id == jobId).Select(j => j.ApplicationsLimit).FirstOrDefaultAsync();
        return jobApplicationLimit;
    }
    
    public async Task<JobType> GetJobTypeAsync( int jobId) {
        JobType jobType = await context.Jobs.Where(j => j.Id == jobId).Select(j => j.Type).FirstOrDefaultAsync();
        return jobType;
    }

    public async Task<bool> SetJobStatusAsync(int jobId, JobStatus status) {
        try {
            await context.Jobs.Where(job => job.Id == jobId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(job => job.Status, status));
            return true;
        }
        catch (DbUpdateConcurrencyException) {
            return false;
        }

        return false;
    }
    
    public async Task<List<JobCardDto>?> GetRelevantJobsAsync(Guid clientId, Roles  role) {
        List<JobCardDto>? relevantJobCards = new List<JobCardDto>();
        switch (role) {
            case Roles.Hirer:
                relevantJobCards = await context.Jobs.Include(j => j.Address).Where(j => j.HirerId == clientId).Where(j => j.Status != JobStatus.Closed).Select(job => new JobCardDto() {
                    Title = job.Title,
                    Status = job.Status,
                    Type = job.Type,
                    WorkMode = job.WorkMode,
                    CompanyName = job.CompanyName,
                    Address = job.Address,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                }).ToListAsync();
                break;
            
            case Roles.Seeker:
                List<string> keywords = await context.Projects
                    .Where(p => p.UserId == clientId)
                    .SelectMany(p => p.ProjectTechnologies.Select(pt => pt.Technology.Name))
                    .Distinct()
                    .ToListAsync();
                // relevantJobs = new List<Job>();
                foreach (var keyword in keywords) {
                    var jobsPerKeyword = await context.Jobs.Include(j => j.Address).Where(j => j.Description.Contains(keyword)).Select(job => new JobCardDto() {
                        Title = job.Title,
                        Status = job.Status,
                        Type = job.Type,
                        WorkMode = job.WorkMode,
                        CompanyName = job.CompanyName,
                        Address = job.Address,
                        MinSalary = job.MinSalary,
                        MaxSalary = job.MaxSalary,
                    }).ToListAsync();

                    relevantJobCards.AddRange(jobsPerKeyword);
                }
                break;
        }
        return relevantJobCards;
    }

    public async Task<bool> CreateJobAsync(Guid hirerId, CreateJobDto newJob) {
        try {
            // TODO - Make a custom class/dto for company to avoid unnecessary fields..
            Address? companyAddress = await context.Hirers.Where(u => u.Id == hirerId).Select(u => u.CompanyAddress).FirstOrDefaultAsync();
            string? companyName = await context.Hirers.Where(u => u.Id == hirerId).Select(u => u.CompanyName).FirstOrDefaultAsync();
            int addressId = await context.Hirers.Select(h => h.CompanyAddressId).FirstOrDefaultAsync();
            if (companyAddress == null || string.IsNullOrEmpty(companyName)) { 
                throw new Exception("Hirer not found");
            }
            await context.Jobs.AddAsync(new Job() {
                Id = await context.Jobs.CountAsync(),    
                Title = newJob.Title, 
                Description = newJob.Description,
                TermsAndConditions = newJob.TermsAndConditions,
                Responsibilities = newJob.Responsibilities,
                RequiredWorkExperience = newJob.RequiredWorkExperience,
                CompanyName = companyName,
                MinSalary = newJob.MinSalary, 
                MaxSalary = newJob.MaxSalary, 
                
                Type = newJob.Type,
                Status = newJob.Status, 
                WorkMode = newJob.WorkMode,
                
                ApplicationsLimit = newJob.ApplicationsLimit,
                PostDate = DateOnly.FromDateTime(DateTime.Today),
                AddressId = addressId,
                HirerId =  hirerId, 
                });
            await context.SaveChangesAsync();

            // TODO - Configure the new job object from the CreateJobDTO. - DONE
            // TODO - Refactor the Job Table and Other respective classes
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }

    public async Task<List<JobForMapMarkerDto>?> GetNearbyJobsAsync(Guid clientId, Roles role,Decimal searchDistance) {
        LocationDto? targetLocation = await context.Users.Where(u => u.Id == clientId).Include(u => u.Address).Select(u => new LocationDto(u.Address.Latitude, u.Address.Longitude)).FirstOrDefaultAsync();
        List<JobCardDto>? relevantJobCards = await GetRelevantJobsAsync(clientId, role);
        if (targetLocation == null) {
            throw new Exception("User doesn't exist!");
        }
        if (relevantJobCards == null) {
            return null;
        }
        
        List<JobForMapMarkerDto>? nearbyJobs = new List<JobForMapMarkerDto>();
        foreach (var job in relevantJobCards) {
            decimal distance = (decimal)Math.Sqrt(Math.Pow((double)(job.Address.Latitude - targetLocation.Latitude), 2) + Math.Pow((double)(job.Address.Latitude - targetLocation.Latitude), 2));
            if (distance < searchDistance) {
                nearbyJobs.Add(new JobForMapMarkerDto() {
                    Id = job.Id,
                    Title = job.Title,
                    Distance = (decimal) Math.Sqrt(Math.Pow((double) (job.Address.Latitude - targetLocation.Latitude), 2) + Math.Pow((double) (job.Address.Latitude - targetLocation.Latitude), 2)),
                    Address =  job.Address
                });
            }
        }
        return nearbyJobs;
    }

    public async Task<JobDescriptionDto?> GetJobDescriptionByIdAsync(int id) {
        Job? job = await context.Jobs.Include(j => j.Address).Where(j => j.Id == id).Select(j => j).FirstOrDefaultAsync();
        if(job == null) {
            throw new Exception("Job not found");
        }
        
        return new JobDescriptionDto() {
            Id = job.Id,
            Title = job.Title,
            CompanyName = job.CompanyName,
            Description = job.Description,
            TermsAndConditions = job.TermsAndConditions,
            Responsibilites = job.Responsibilities,
            MinSalary = job.MinSalary,
            MaxSalary = job.MaxSalary,
            
            Address = job.Address,
            
            Type = job.Type,
            Status = job.Status,
            WorkMode = job.WorkMode,
            
            PostDate = job.PostDate,
            NumberOfApplicants = context.Applications.Count(a => a.HirerId == job.HirerId),
        };
    }
}