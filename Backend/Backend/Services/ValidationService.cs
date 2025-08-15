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
        await context.Jobs.Where(job => job.Id == jobId).Select(job => job.HirerId).FirstOrDefaultAsync() == userId;

    public async Task<bool> JobExistsAsync(int jobId) =>
        await context.Jobs.AnyAsync(j => j.Id == jobId);

    public async Task<bool> InterviewExistsAsync(int interviewId) =>
        await context.Interviews.AnyAsync(i => i.Id == interviewId);

    public async Task<bool> ApplicationExistsAsync(int applicationId) =>
        await context.Applications.AnyAsync(a => a.Id == applicationId);

    public async Task<bool> IsTheirApplicationAsync(Guid userId, int applicationId) {
        Guid seekerId = await context.Applications.Where(application => application.Id == applicationId)
            .Select(application => application.SeekerId).FirstOrDefaultAsync();
        Guid hirerId = await context.Applications.Where(application => application.Id == applicationId)
            .Select(application => application.HirerId).FirstOrDefaultAsync();
        return seekerId == userId || hirerId == userId;
    }

    public async Task<bool> IsTheirInterviewAsync(Guid userId, int interviewId) {
        Guid seekerId = await context.Interviews.Where(interview => interview.Id == interviewId)
            .Select(interview => interview.SeekerId).FirstOrDefaultAsync();
        Guid hirerId = await context.Interviews.Where(interview => interview.Id == interviewId)
            .Select(interview => interview.HirerId).FirstOrDefaultAsync();
        return seekerId == userId || hirerId == userId;
    }

    // public async Task<bool> GithubProjectsAlreadyStored(Guid userId) {
    //     // GitHub Username is set to something and even some projects are added
    //     if (!(await context.Users.Where(user => user.Id == userId).Select(user => user.GithubUsername).FirstOrDefaultAsync())!.Equals("") && (await context.Projects.Where(proj => proj.UserId == userId).ToListAsync()).Count != 0) {
    //         return true;
    //     }
    //
    //     return false;
    // }

    public async Task<bool> CanAddMoreProjects(Guid userId) {
        if ((await context.Projects.Where(project => project.UserId == userId).ToListAsync()).Count <= 3) {
            return true;
        }

        return false;
    }
}