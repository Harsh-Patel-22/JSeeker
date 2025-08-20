using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Models.Users;
using Backend.Util;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class JobRepository (ApplicationDbContext context) {
    
    public async Task<List<JobCardDto>?> GetRelevantJobsBySearchFilterAsync(Guid clientId, Roles role, JobSearchFilterDto searchFilter) {
        List<JobCardDto>? relevantJobCards = new List<JobCardDto>();
        switch (role) {
            case Roles.Hirer:
                // TODO - What if for each parameter there's more filters
                relevantJobCards = await context.Jobs.Include(j => j.Address).Where(j => j.HirerId == clientId  && j.Type == searchFilter.type && j.Status == searchFilter.status && j.WorkMode == searchFilter.mode).Select(job => new JobCardDto() {
                    Title = job.Title,
                    Status = job.Status,
                    Type = job.Type,
                    WorkMode = job.WorkMode,
                    CompanyName = job.CompanyName,
                    Address = job.Address,
                    MinSalary = job.MinSalary,
                    MaxSalary = job.MaxSalary,
                    NumberOfApplications = job.NumberOfApplications,
                    PostDate = job.PostDate
                }).ToListAsync();
                break;
            
            case Roles.Seeker:
                // TODO - Reduce the confidence of the AI Generated Keywords. Give prio to the proper keywords...
                string[]? keywordsRecord = await context.Users.Where(user => user.Id == clientId).Select(user => new string[]{user.TechnicalKeywords, user.AIGeneratedTechnicalKeywords}).FirstOrDefaultAsync();
                if (keywordsRecord == null) {
                    throw new Exception("User or keywords not found");
                };
                string keywordsCSV = keywordsRecord[0] + keywordsRecord[1];
                List<string> keywords = keywordsCSV.Split(",").ToList();
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
                        NumberOfApplications = job.NumberOfApplications,
                        PostDate = job.PostDate
                    }).ToListAsync();

                    relevantJobCards.AddRange(jobsPerKeyword);
                }
                break;
        }
        return relevantJobCards;
    }
    

    public async Task<bool> CreateJobAsync(Guid hirerId, CreateJobDto newJob) {
        try {
            if (!Enum.TryParse(newJob.Type, out JobType jobType)) {
                throw new Exception("Invalid job type");
            }

            if (!Enum.TryParse(newJob.WorkMode, out WorkMode workMode)) {
                throw new Exception("Invalid work mode");
            }
            // TODO - Make a custom class/dto for company to avoid unnecessary fields..
            string? companyName = await context.Hirers.Where(u => u.Id == hirerId).Select(u => u.CompanyName).FirstOrDefaultAsync();
            int addressId = await context.Hirers.Where(h => h.Id == hirerId).Select(h => h.CompanyAddressId).FirstOrDefaultAsync();
            // companyName = "DUAL SHARD";
            
            // TODO - Remove this code once the hirer having company details is set!!
            if (string.IsNullOrEmpty(companyName)) { 
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
                
                Type = jobType,
                Status = JobStatus.Open, 
                WorkMode = workMode,
                
                NumberOfApplications = 0,
                ApplicationsLimit = newJob.ApplicationsLimit,
                PostDate = DateOnly.FromDateTime(DateTime.Today),
                AddressId = addressId,
                HirerId =  hirerId, 
                });
            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception e) { // This catch also catches the custom thrown exceptions....
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<List<JobForMapMarkerDto>?> GetNearbyJobsAsync(Guid clientId, Roles role, Decimal searchDistance, JobSearchFilterDto searchFilter) {
        LocationDto? targetLocation = await context.Users.Where(u => u.Id == clientId).Include(u => u.Address).Select(u => new LocationDto(u.Address.Latitude, u.Address.Longitude)).FirstOrDefaultAsync();
        List<JobCardDto>? relevantJobCards = await GetRelevantJobsBySearchFilterAsync(clientId, role, searchFilter);
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
            return null;
        }
        
        return new JobDescriptionDto() {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            TermsAndConditions = job.TermsAndConditions,
            Responsibilites = job.Responsibilities,
            RequiredWorkExperience = job.RequiredWorkExperience,
            CompanyName = job.CompanyName,
            MinSalary = job.MinSalary,
            MaxSalary = job.MaxSalary,
            
            Address = job.Address,
            
            Type = job.Type,
            Status = job.Status,
            WorkMode = job.WorkMode,
            
            PostDate = job.PostDate,
            NumberOfApplicants = job.NumberOfApplications,
            ApplicationsLimit = job.ApplicationsLimit
        };
    }

    public async Task<bool> EditJobAsync(int jobId, EditJobDto dto) {
        // await context.Jobs.Where(job => job.Id == dto.Id).
        if (!Enum.TryParse(dto.Status, out JobStatus status)) {
            throw new Exception("Invalid status");
        }if (!Enum.TryParse(dto.Type, out JobType type)) {
            throw new Exception("Invalid type");
        }if (!Enum.TryParse(dto.WorkMode, out WorkMode mode)) {
            throw new Exception("Invalid mode");
        }
        Job job = new Job() {
            Id = jobId,
            Title = dto.Title,
            Description = dto.Description,
            TermsAndConditions = dto.TermsAndConditions,
            Responsibilities = dto.Responsibilities,
            RequiredWorkExperience = dto.RequiredWorkExperience,
            MinSalary = dto.MinSalary,
            MaxSalary = dto.MaxSalary,

            Type = type,
            Status = status,
            WorkMode = mode,

            ApplicationsLimit = dto.ApplicationsLimit
        };
        return await DbUpdateHelper.UpdateAllFieldsExceptAsync(job, context, "Id","CompanyName", "PostDate", "NumberOfApplications", "AddressId", "HirerId");
        // return true;
    }
    
    
    // Helper Methods - Getters/Setters
    // public async Task<int> GetApplicationsCountAsync(int jobId) {
    //     return await context.Jobs.Where(j => j.Id == jobId).Select(j => j.NumberOfApplications).FirstOrDefaultAsync();
    // }

    public async Task<JobKeyInformationDto> GetJobKeyInformationByIdAsync(int id) {
        
        JobKeyInformationDto? jobKeyInformation = await context.Jobs.Where(job =>  job.Id == id).Select(j => new JobKeyInformationDto(j.Status, j.Type, j.ApplicationsLimit)).FirstOrDefaultAsync();
        if(jobKeyInformation == null) throw new Exception("Job not found");
        
        return jobKeyInformation;
    }
    public async Task<int> UpdateApplicationsCountAsync(int jobId) {
         await context.Jobs.Where(job => job.Id == jobId).ExecuteUpdateAsync(setter => setter.SetProperty(j => j.NumberOfApplications, j => j.NumberOfApplications + 1));
         return await context.Jobs.Where(job => job.Id == jobId).Select(job => job.NumberOfApplications).FirstOrDefaultAsync();
    }
    
    // public async Task<int> GetJobApplicationLimitAsync(int jobId) {
    //     int jobApplicationLimit = await context.Jobs.Where(j => j.Id == jobId).Select(j => j.ApplicationsLimit).FirstOrDefaultAsync();
    //     return jobApplicationLimit;
    // }
    //
    // public async Task<JobType> GetJobTypeAsync( int jobId) {
    //     JobType jobType = await context.Jobs.Where(j => j.Id == jobId).Select(j => j.Type).FirstOrDefaultAsync();
    //     return jobType;
    // }
    //
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

}