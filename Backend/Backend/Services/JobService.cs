using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Models.Users;
using Backend.Repositories;

namespace Backend.Services;

public class JobService (JobRepository jobRepository, InterviewRepository interviewRepository, ApplicationRepository applicationRepository) {
    
    private readonly int _remainingApplicationsThresholdForClosingSoon = 10; 
    // Section - Job Related

    private async Task<bool> CheckIfAppliableAsync( JobType seekingFor, int jobId) {
        JobType jobType = await jobRepository.GetJobTypeAsync(jobId);
        if (seekingFor != jobType) return false;
        int applicationLimit = await jobRepository.GetJobApplicationLimitAsync(jobId);
        int applicationsCount = await applicationRepository.GetApplicationsCountAsync(jobId);

        
        // TODO - Add the below if after the application creation
        if (applicationLimit - applicationsCount <= _remainingApplicationsThresholdForClosingSoon * applicationLimit / 100f) {
            await jobRepository.SetJobStatusAsync(jobId, JobStatus.ClosingSoon);   
        }
        
        else if (applicationLimit < applicationsCount) {
            await jobRepository.SetJobStatusAsync(jobId, JobStatus.Closed);
            return false;
        }
        

            return true;
    }
    public async Task<List<JobCardDto>?> GetRelevantJobsAsync(Guid clientId, Roles role) {
        return await jobRepository.GetRelevantJobsAsync(clientId, role);
    }

    public async Task<bool> CreateJobAsync(Guid hirerId, CreateJobDto newJob) {
        return await jobRepository.CreateJobAsync(hirerId, newJob); 
    }

    public async Task<List<JobForMapMarkerDto>?> GetNearbyJobs(Guid clientId, Roles role, decimal searchRadius) {
        return await jobRepository.GetNearbyJobsAsync(clientId, role, searchRadius);
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