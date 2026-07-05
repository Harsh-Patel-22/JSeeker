using Backend.Data;
using Backend.Models.Users;
using Backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ValidationService(ApplicationDbContext context) {
    public async Task<bool> UserExistsAsync(Guid userId) =>
        await context.Users.AnyAsync(u => u.Id == userId);

    public async Task<bool> IsHirerAsync(Guid userId) =>
        await context.Users.AnyAsync(u => u.Id == userId && u.IsHirer);

    public async Task<bool> IsTheirJobAsync(Guid userId, int jobId) =>
        await context.Jobs.Where(job => job.Id == jobId && job.HirerId == userId).AnyAsync();

    public async Task<bool> JobExistsAsync(int jobId) =>
        await context.Jobs.AnyAsync(j => j.Id == jobId);

    public async Task<bool> InterviewExistsAsync(int interviewId) =>
        await context.Interviews.AnyAsync(i => i.ApplicationId == interviewId);

    public async Task<bool> ApplicationExistsAsync(int applicationId) =>
        await context.Applications.AnyAsync(a => a.ApplicationId == applicationId);

    public async Task<bool> IsTheirApplicationAsync(Guid userId, int applicationId) {
        var ids = await context.Applications
            .Where(appl => appl.ApplicationId == applicationId)
            .Select(appl =>
            new Guid[] {
                appl.SeekerId,
                appl.HirerId
            })
            .FirstOrDefaultAsync();
        // Guid seekerId = await context.Applications.Where(application => application.ApplicationId == applicationId)
        //     .Select(application => application.SeekerId).FirstOrDefaultAsync();
        // Guid hirerId = await context.Applications.Where(application => application.ApplicationId == applicationId)
        //     .Select(application => application.HirerId).FirstOrDefaultAsync();
        return ids != null && (ids[0] == userId || ids[1] == userId);
    }
    

    public async Task<bool> IsTheirInterviewAsync(Guid userId, int interviewId) {
        var ids = await context.Interviews
            .Where(i => i.ApplicationId == interviewId)
            .Select(i =>
            new Guid[] {
                i.SeekerId,
                i.HirerId
            })
            .FirstOrDefaultAsync();
        // Guid seekerId = await context.Interviews.Where(interview => interview.ApplicationId == interviewId)
        //     .Select(interview => interview.SeekerId).FirstOrDefaultAsync();
        // Guid hirerId = await context.Interviews.Where(interview => interview.ApplicationId == interviewId)
        //     .Select(interview => interview.HirerId).FirstOrDefaultAsync();
        return ids != null && (ids[0] == userId || ids[1] == userId);
    }

    public async Task<bool> HasApplicationFor(Guid seekerId, Guid hirerId) => 
        await context.Applications.AnyAsync(appl => appl.SeekerId == seekerId && appl.HirerId == hirerId);
    public async Task<bool> HasInterviewFor(Guid seekerId, Guid hirerId) => 
        await context.Applications.AnyAsync(appl => appl.SeekerId == seekerId && appl.HirerId == hirerId);


    public async Task<bool> ApplicationAlreadyExistAsync(Guid userId, int jobId) => await context.Applications
        .Where(appl => appl.SeekerId == userId && appl.JobId == jobId).AnyAsync();
    public async Task<bool> InterviewAlreadyExistAsync(Guid userId, int jobId) => await context.Interviews
        .Where(appl => appl.SeekerId == userId && appl.JobId == jobId).AnyAsync();

    public async Task<bool> CanAddMoreProjects(Guid userId) =>
        await context.Projects
            .Where(project => project.UserId == userId)
            .CountAsync() <= 3;
}