using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ApplicationRepository (ApplicationDbContext context) {
    public async Task<List<ApplicationDto>> GetAllApplicationsAsync(Guid userId) {
        var applications = await context.Applications.Where(application => application.HirerId == userId).Select(application => new ApplicationDto() {
            ApplicantId = application.ApplicantId,
            JobId = application.JobId,
            HirerId = application.HirerId,
        }).ToListAsync();
        return applications;
    }


    public async Task<bool> CreateApplicationAsync(CreateApplicationDto newApplicationDto) {
        try {
            await context.Applications.AddAsync(new Application() {
                ApplicantId = newApplicationDto.ApplicantId,
                JobId = newApplicationDto.JobId,
                HirerId = newApplicationDto.HirerId,
            });
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }
}