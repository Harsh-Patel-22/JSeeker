using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Query;

public class MetricsQueryService(ApplicationDbContext context) {
    // TODO - A count of scheduled interviews and applications could also be given...
    
    private async Task<int> GetTotalNumberOfRegisteredUsersAsync() {
        return await context.UserCredentials.CountAsync();
    }

    private async Task<int> GetTotalNumberOfJobsAsync() {
        return await context.Jobs.CountAsync();
    }

    private async Task<float> GetAverageNumberOfJobsPostedDailyAsync() {
        int lastNYears = 5;
        var cutoffDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-lastNYears));
        
        // Fetch only the Year and Month of jobs in the last 5 years in a single DB query
        var jobDates = await context.Jobs
            .Where(job => job.PostDate >= cutoffDate)
            .Select(job => new { job.PostDate.Year, job.PostDate.Month })
            .ToListAsync();
            
        float totalAverages = 0f;
        int currentYear = DateTime.Now.Year;
        
        for (int year = currentYear; year > currentYear - lastNYears; year--) {
            float monthlySum = 0f;
            for (int month = 1; month <= 12; month++) {
                int count = jobDates.Count(jd => jd.Year == year && jd.Month == month);
                monthlySum += count;
            }
            totalAverages += (monthlySum / 12f);
        }
        
        return totalAverages / lastNYears;
    }

    private async Task<int> GetNumberOfSuccessfulJobLandingsAsync() {
        return await context.Users.Select(user => user.NumberOfSuccessfulEmployments).SumAsync();
    }

    private async Task<int> GetNumberOfRejectionsAsync() {
        return await context.Users.Select(user => user.NumberOfRejections).SumAsync();
    }

    public async Task<MetricsDto> GetAllMetricsAsync() {
        int totalUsers = await GetTotalNumberOfRegisteredUsersAsync();
        int totalJobs = await GetTotalNumberOfJobsAsync();
        float avgJobs = await GetAverageNumberOfJobsPostedDailyAsync();
        int successLandings = await GetNumberOfSuccessfulJobLandingsAsync();
        int rejections = await GetNumberOfRejectionsAsync();
        
        float successRate = (successLandings + rejections) > 0 
            ? (float)successLandings / (successLandings + rejections) 
            : 0f;

        return new MetricsDto(
            totalUsers,
            totalJobs,
            avgJobs,
            successLandings,
            successRate
        );
    }
    
}