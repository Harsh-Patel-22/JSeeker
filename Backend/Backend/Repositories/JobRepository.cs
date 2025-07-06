using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class JobRepository (ApplicationDbContext context) {
    
    public async Task<List<JobCardDto>> GetRelevantJobsAsync(Guid clientId) {
        List<Job> relevantJobs = await context.Jobs.ToListAsync();
        List<JobCardDto> relevantJobCards = new List<JobCardDto>();
        
        foreach (Job job in relevantJobs) {
            relevantJobCards.Add(new JobCardDto() {
                Id = job.Id,
                Title = job.Title,
                Status = job.Status,
                WorkMode = "On-Site",
                Location = (from location in context.Locations where location.Id == job.LocationId select location).First(),
                MinSalary = job.Salary,
                MaxSalary = job.Salary + 100000,
            });
        }
        return relevantJobCards;
    }

    public async Task<bool> CreateJobAsync(CreateJobDto newJob) {
        try {
            await context.Jobs.AddAsync(new Job(context.Jobs.Count(), newJob.Title, newJob.Description,
                newJob.TermsAndConditions, newJob.Salary, newJob.Status, newJob.LocationId) {
                Title = string.Empty,
                Description = string.Empty
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

    public async Task<List<JobForMapMarkerDto>> GetNearbyJobsAsync(Location targetLocation, Decimal searchDistance) {
        List<JobForMapMarkerDto> nearbyJobs = await context.Jobs.Include(job => job.Location).Where(job => (decimal) Math.Sqrt(Math.Pow((double) (job.Location.Latitude - targetLocation.Latitude), 2) + Math.Pow((double) (job.Location.Latitude - targetLocation.Latitude), 2)) < searchDistance).Select(j => 
            new JobForMapMarkerDto() { 
                Id = j.Id,
                Title = j.Title,
                Distance = (decimal) Math.Sqrt(Math.Pow((double) (j.Location.Latitude - targetLocation.Latitude), 2) + Math.Pow((double) (j.Location.Latitude - targetLocation.Latitude), 2)),
                Location = j.Location
            }).ToListAsync();
        return nearbyJobs;
    }

    public async Task<JobDescriptionDto?> GetJobDescriptionByIdAsync(int id) {
        Job? job = await context.Jobs.FindAsync(id);
        if(job == null) {
            return null;
        }
        
        return new JobDescriptionDto() {
            Id = job.Id,
            Title = job.Title,
            Status = job.Status,
            WorkMode = "On-Site",
            MinSalary = job.Salary,
            MaxSalary = job.Salary + 100000,
            Location = (from location in context.Locations where location.Id == job.LocationId select location).First(),
            
            Description = job.Description,
            TermsAndConditions = job.TermsAndConditions,
            Requirements = "Requirements",
            Miscellaneous = "lorem32",
            
            PostedDaysAgo = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            NumberOfApplicants = context.Applications.Count(),
        };
    }
}