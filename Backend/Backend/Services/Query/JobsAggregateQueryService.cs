using Backend.Data;
using Backend.DTOs.Job;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Query;

public class JobsAggregateQueryService (ApplicationDbContext context) {
    public async Task<Dictionary<int, List<ApplicationDto>>> GetAllApplicationsByUserIdJobWiseAsync(Guid hirerId) {
        Dictionary<int, List<ApplicationDto>> jobWiseApplications = new Dictionary<int, List<ApplicationDto>>();
        var jobsIdList = await context.Jobs.Where(job => job.HirerId == hirerId).Select(job => job.Id).ToListAsync();
        foreach (var jobId in jobsIdList) {
            var applications = await context.Applications.Include(app => app.Job).Include(app => app.Seeker).Where(app => app.JobId == jobId).Select(app => new ApplicationDto() {
                ApplicationId = app.ApplicationId,
                ApplicantId = app.SeekerId,
                HirerId = app.HirerId,
                SeekerId = app.SeekerId,
                JobId = app.JobId,
                State = app.State,
                
                JobTitle = app.Job.Title,
                AIGivenRating = app.AIGivenRating,
                AppliedOn = app.AppliedOn,
                
                Email = app.Seeker.Email,
                FirstName = app.Seeker.FirstName,
                LastName = app.Seeker.LastName,
                PhoneNumber = app.Seeker.PhoneNumber,
                
                ResumeJsonString = app.Seeker.ResumeJsonString,
                ResumeTemplateNumber = app.Seeker.ResumeTemplateNumber
            }).ToListAsync();
            
            jobWiseApplications.Add(jobId, applications);
        }
        return jobWiseApplications;
    }
    
    public async Task<Dictionary<int, List<InterviewDto>>> GetAllInterviewsByUserIdJobWiseAsync(Guid hirerId) {
        Dictionary<int, List<InterviewDto>> jobWiseInterviews = new Dictionary<int, List<InterviewDto>>();
        var jobsIdList = await context.Jobs.Where(job => job.HirerId == hirerId).Select(job => job.Id).ToListAsync();
        foreach (var jobId in jobsIdList) {
            var interviews = await context.Interviews.Include(i => i.Job).Include(i => i.Seeker).Where(i => i.JobId == jobId).Select(i => new InterviewDto() {
                Id = i.ApplicationId,
                JobId = i.JobId,
                SeekerId = i.SeekerId,
                HirerId = i.HirerId,
                
                // Seeker Related
                FirstName = i.Seeker.FirstName,
                LastName = i.Seeker.LastName,
                Email = i.Seeker.Email,
                PhoneNumber = i.Seeker.PhoneNumber,
                // GithubProjectLinks = interview.Seeker.Projects
                    
                // Job Related
                JobTitle = i.Job.Title,
                CompanyName = i.Job.CompanyName,
                JobDescription = i.Job.Description,
                JobResponsibilities = i.Job.Responsibilities,
                JobTermsAndConditions = i.Job.TermsAndConditions,
                
                Date = i.Date,
                Time = i.Time,
                Mode = i.Mode,
                ConfirmedByHirer = i.ConfirmedByHirer,
                ConfirmedBySeeker = i.ConfirmedBySeeker,
            }).ToListAsync();
            
            jobWiseInterviews.Add(jobId, interviews);
        }
        return jobWiseInterviews;
    }
}