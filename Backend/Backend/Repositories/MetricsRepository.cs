using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class MetricsRepository(ApplicationDbContext context) {
    // TODO - A count of scheduled interviews and applications could also be given...
    
    private async Task<int> GetTotalNumberOfRegisteredUsersAsync() {
        return await context.UserCredentials.CountAsync();
    }

    private async Task<int> GetTotalNumberOfJobsAsync() {
        return await context.Jobs.CountAsync();
    }

    private async Task<float> GetAverageNumberOfJobsPostedDailyAsync() {
        // TODO - Find a way to make it better - SumAsync() and AverageAsync()
        float monthlyAverages = 0f;
        int currentYear = DateTime.Now.Year;
        int lastNYears = 5;
        float totalAverages = 0f;
        
        for (int year = currentYear; lastNYears >= currentYear - year; year--) {
            for (int i = 1; i <= 12; i++) {
                var count = await context.Jobs.Where(job => job.PostDate.Month == i && job.PostDate.Year == year).CountAsync();
                monthlyAverages += count;
            }
            monthlyAverages /= 12;
            totalAverages += monthlyAverages;
            monthlyAverages = 0f;
        }
        totalAverages /= lastNYears;
        
        return totalAverages;
    }

    private async Task<int> GetNumberOfSuccessfulJobLandingsAsync() {
        return await context.Users.Select(user => user.NumberOfSuccessfulEmployments).SumAsync();
    }

    private async Task<int> GetNumberOfRejectionsAsync() {
        return await context.Users.Select(user => user.NumberOfRejections).SumAsync();
    }

    private async Task<float> GetJobLandingSuccessRateAsync() {
        int successNumber = await GetNumberOfSuccessfulJobLandingsAsync();
        int failureNumber = await GetNumberOfRejectionsAsync();
        return (float) successNumber / (successNumber + failureNumber);
    }

    public async Task<MetricsDto> GetAllMetricsAsync() {
        return new MetricsDto(
            await GetTotalNumberOfRegisteredUsersAsync(),
            await GetTotalNumberOfJobsAsync(),
            await GetAverageNumberOfJobsPostedDailyAsync(),
            await GetNumberOfSuccessfulJobLandingsAsync(),
            await GetJobLandingSuccessRateAsync()
            );
    }
    
}