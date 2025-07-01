using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class JobRepository (ApplicationDbContext context) {
    
    public List<JobCardDto> GetRelevantJobs(int clientId) {
        List<Job> relevantJobs = context.Jobs.ToList();
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
    
    // public 
}