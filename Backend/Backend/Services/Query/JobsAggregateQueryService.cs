using Backend.Data;
using Backend.DTOs.Job;
using Backend.DTOs.Users.Hirer;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Query;

public class JobsAggregateQueryService (ApplicationDbContext context) {
    public async Task<Dictionary<int, List<ApplicationDto>>> GetAllApplicationsByUserIdJobWiseAsync(Guid hirerId) {
        Dictionary<int, List<ApplicationDto>> jobWiseApplications = new Dictionary<int, List<ApplicationDto>>();
        var jobsIdList = await context.Jobs.Where(job => job.HirerId == hirerId).Select(job => job.Id).ToListAsync();
        foreach (var jobId in jobsIdList) {
            var applications = await context.Applications.Include(app => app.Job).Include(app => app.Seeker).Where(app => app.JobId == jobId).Select(app => new ApplicationDto() {
                ApplicationId = app.ApplicationId,
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
                ResumeTemplateNumber = app.Seeker.ResumeTemplateNumber,
                Technologies = app.Seeker.TechnicalKeywords,
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

    public async Task<HirerDashboardMetricsDto> GetHirerDashboardMetricsAsync(Guid hirerId) {
        var totalHires = await context.Interviews.Where(i => i.HirerId == hirerId && i.Outcome == InterviewOutcome.Hired)
            .ToListAsync();
        var numActiveJobOpenings = await context.Jobs
            .Where(job => job.HirerId == hirerId && job.Status != JobStatus.Closed).ToListAsync();
        var numNewApplicationsToday = await context.Applications
            .Where(appl => appl.HirerId == hirerId && appl.AppliedOn == DateOnly.FromDateTime(DateTime.Today)).ToListAsync();
        
        // var numRejectedFromApplication = await context.Applications.Where(appl => appl.HirerId == hirerId && appl.State == ApplicationState.Rejected).ToListAsync();
        // var numRejectedFromInteview = await context.Interviews.Where(appl => appl.HirerId == hirerId && appl.Outcome == InterviewOutcome.Rejected).ToListAsync();
        var numTotalApplications = await context.Applications.Where(appl => appl.HirerId == hirerId).ToListAsync();
        
        var dto = new HirerDashboardMetricsDto() {
            NumActiveJobOpenings = numActiveJobOpenings.Count,
            NumNewApplicationsToday = numNewApplicationsToday.Count,
            TotalHires = totalHires.Count,
            HiringRate = (decimal) totalHires.Count / numTotalApplications.Count,
            ScheduledInterviews = await context.Interviews.Where(i => i.HirerId == hirerId && i.ConfirmedByHirer && i.ConfirmedBySeeker && i.Date == DateOnly.FromDateTime(DateTime.Today)).Select(i => new InterviewBasicDetailsDto() {
                FirstName = i.Seeker.FirstName,
                LastName = i.Seeker.LastName,
                Date = i.Date,
                Time = i.Time,
                Mode = i.Mode,
            }).ToListAsync()
        };
        return dto;
    }
    
    public async Task<string?> GetUserStateFromAddressAsync(Guid userId) {
        return  await context.Users.Where(user => user.Id == userId).Include(u => u.Address).Select(u => u.Address.State).FirstOrDefaultAsync(); 
    }
}