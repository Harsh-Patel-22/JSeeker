using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ApplicationRepository (ApplicationDbContext context) {
    public async Task<List<ApplicationDto>> GetAllApplicationsByHirerIdByStateAsync(Guid userId, ApplicationState applicationState) {
        var applications = await context.Applications.Include(application => application.Job).Include(application => application.Seeker).ThenInclude(seeker => seeker.Projects).Where(application => application.HirerId == userId && application.State == applicationState).Select(application => new ApplicationDto() {
            ApplicantId = application.SeekerId,
            JobId = application.JobId,
            HirerId = application.HirerId,
            FirstName = application.Seeker.FirstName,
            LastName = application.Seeker.LastName,
            Email = application.Seeker.Email,
            PhoneNumber = application.Seeker.PhoneNumber,
            State = application.State,
            AppliedOn =  application.AppliedOn,
            AIGivenRating = application.AIGivenRating,
            PrecreatedResume = application.PreCreatedResume,
            ResumeJsonString = application.Seeker.ResumeJsonString,
            ResumeTemplateNumber = application.Seeker.ResumeTemplateNumber,
            // TODO - Fix the technologies field
            // Technologies = application.Seeker.
        }).ToListAsync();
        return applications;
    }

    public async Task UpdateApplicationStateAsync(int applicationId, ApplicationState applicationState) {
        await context.Applications.Where(app => app.Id == applicationId).ExecuteUpdateAsync(setter => setter.SetProperty(app => app.State, applicationState));
    }

    public async Task DeleteApplicationAsync(int applicationId) {
        await context.Applications.Where(app => app.Id == applicationId).ExecuteDeleteAsync();
    }

    public async Task<bool> CreateApplicationAsync(CreateApplicationDto newApplicationDto) {
        try {
            await context.Applications.AddAsync(new Application() {
                SeekerId = newApplicationDto.SeekerId,
                JobId = newApplicationDto.JobId,
                HirerId = newApplicationDto.HirerId,
                State = ApplicationState.Pending,
                AIGivenRating = newApplicationDto.AIRating
            });
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }

    public async Task<ApplicationDto?> GetApplicationByIdAsync(int applicationId) {
        return await context.Applications.Include(application => application.Job).Include(application => application.Seeker).Where(application => application.Id == applicationId).Select(application => new ApplicationDto() {
            ApplicantId = application.SeekerId,
            JobId = application.JobId,
            HirerId = application.HirerId,
            FirstName = application.Seeker.FirstName,
            LastName = application.Seeker.LastName,
            Email = application.Seeker.Email,
            PhoneNumber = application.Seeker.PhoneNumber,
            State = application.State,
            AppliedOn = application.AppliedOn,
            AIGivenRating = application.AIGivenRating,
            PrecreatedResume = application.PreCreatedResume,
            ResumeJsonString = application.Seeker.ResumeJsonString,
            ResumeTemplateNumber = application.Seeker.ResumeTemplateNumber,
        }).FirstOrDefaultAsync();
    }

    public async Task<Guid> GetSeekerIdByApplicationIdAsync(int applicationId) {
        return await context.Applications.Where(appl => appl.Id == applicationId).Select(appl => appl.SeekerId).FirstOrDefaultAsync();
    }
}
