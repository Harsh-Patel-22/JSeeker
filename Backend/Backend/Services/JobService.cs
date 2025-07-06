using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class JobService (JobRepository jobRepository, InterviewRepository interviewRepository, ApplicationRepository applicationRepository) {
    
    // Section - Job Related
    public async Task<List<JobCardDto>> GetRelevantJobsAsync(Guid clientId) {
        return await jobRepository.GetRelevantJobsAsync(clientId);
    }

    public async Task<bool> CreateJobAsync(CreateJobDto newJob) {
        return await jobRepository.CreateJobAsync(newJob); 
    }

    public async Task<List<JobForMapMarkerDto>> GetNearbyJobs(SearchLocationDto searchLocationDto) {
        return await jobRepository.GetNearbyJobsAsync(new Location() {
            Latitude = searchLocationDto.Latitude,
            Longitude = searchLocationDto.Longitude,
        }, searchLocationDto.SearchDistance);
    }

    public async Task<JobDescriptionDto?> GetJobDescriptionByIdAsync(int id) {
        return await jobRepository.GetJobDescriptionByIdAsync(id);
    }
    
    // Section - Interview Related
    public async Task<List<InterviewDto>> GetInterviewsByIdAsync(Guid userId) {
        return await interviewRepository.GetInterviewsByIdAsync(userId);
    }

    public async Task<bool> CreateInterviewAsync(CreateInterviewDto newInterviewDto) {
        return await interviewRepository.CreateInterviewsAsync(newInterviewDto);
    }
    
    // Section - Application Related
    public async Task<List<ApplicationDto>> GetAllApplicationsAsync(Guid userId) {
        return await applicationRepository.GetAllApplicationsAsync(userId);
    }

    public async Task<bool> CreateApplicationAsync(CreateApplicationDto newApplicationDto) {
        return await applicationRepository.CreateApplicationAsync(newApplicationDto);
    }
}