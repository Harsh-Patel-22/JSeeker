using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Models.Users;
using Backend.Repositories;

namespace Backend.Services;

public class JobService (JobRepository jobRepository, InterviewRepository interviewRepository, ApplicationRepository applicationRepository, UserRepository userRepository, RatingService ratingService) {
    
    private readonly int _remainingApplicationsThresholdForClosingSoon = 10; 
    // Section - Job Related

   
    public async Task<List<JobCardDto>?> GetRelevantJobsAsync(Guid clientId, Roles role, JobSearchFilterDto searchFilter) {
        return await jobRepository.GetRelevantJobsBySearchFilterAsync(clientId, role, searchFilter);
    }
    
    public async Task<bool> CreateJobAsync(Guid hirerId, CreateJobDto newJob) {
        return await jobRepository.CreateJobAsync(hirerId, newJob); 
    }

    public async Task<bool> UpdateJobAsync(int jobId, EditJobDto dto) {
        return await jobRepository.EditJobAsync(jobId, dto);
    }

    public async Task<List<JobForMapMarkerDto>?> GetNearbyJobs(Guid clientId, Roles role, decimal searchRadius, JobSearchFilterDto searchFilter) {
        return await jobRepository.GetNearbyJobsAsync(clientId, role, searchRadius, searchFilter);
    }

    public async Task<JobDescriptionDto?> GetJobDescriptionByIdAsync(int id) {
        return await jobRepository.GetJobDescriptionByIdAsync(id);
    }
    
    // Section - Interview Related
    public async Task<List<InterviewDto>> GetInterviewsByIdByScheduleStatusAsync(Guid userId, bool scheduled) {
        return await interviewRepository.GetInterviewsByIdByScheduleStatusAsync(userId, scheduled);
    }

    public async Task<InterviewDto> GetInterviewByIdAsync(int interviewId) {
        var interviewDto = await interviewRepository.GetInterviewByIdAsync(interviewId);
        if (interviewDto == null) {
            throw new Exception("Interview not found");
        }
        return interviewDto;
    }
    
    public async Task UpdateInterviewDateTimeAsync(int interviewId, Roles role, DateAndTimeDto dto) {
        await interviewRepository.UpdateInterviewDateTimeAsync(interviewId, dto);
    }
    
    public async Task<bool> CreateInterviewAsync(CreateInterviewDto newInterviewDto) {
        return await interviewRepository.CreateInterviewsAsync(newInterviewDto);
    }

    public async Task UpdateSeekerSuccessFailureJobLanding(int interviewId, bool successful) {
        Guid userId = await interviewRepository.GetSeekerIdByInterviewId(interviewId);
        if (successful) {
            await userRepository.IncrementSuccessCountAsync(userId);
        }
        else {
            await userRepository.IncrementRejectedCountAsync(userId);
        }
    }
    
    // Section - Application Related
    public async Task<List<ApplicationDto>> GetAllApplicationsByHirerIdByStateAsync(Guid userId, ApplicationState state) {
        return await applicationRepository.GetAllApplicationsByHirerIdByStateAsync(userId, state);
    }

    public async Task<ApplicationDto> GetApplicationByIdAsync(int applicationId) {
        var applicationDto = await applicationRepository.GetApplicationByIdAsync(applicationId);
        if (applicationDto == null) {
            throw new Exception("Application Doesn't Exist");
        }
        return applicationDto;
    }

    public async Task UpdateApplicationStateAsync(ApplicationStateUpdateDto dto) {
        if (dto.State != ApplicationState.Rejected) {
            await applicationRepository.UpdateApplicationStateAsync(dto.ApplicationId, dto.State);
        }
        else {
            Guid userId = await applicationRepository.GetSeekerIdByApplicationIdAsync(dto.ApplicationId);
            await userRepository.IncrementRejectedCountAsync(userId);
        }
    }

    
    public async Task<bool> CheckAndCreateApplicationAsync(CreateApplicationDto newApplicationDto) {
        JobKeyInformationDto job = await jobRepository.GetJobKeyInformationByIdAsync(newApplicationDto.JobId);
        if (newApplicationDto.JobType != job.JobType || job.JobStatus == JobStatus.Closed) return false;
        
        int aiGivenRating = await ratingService.GetAIRatingForApplicationAsync(new ApplicationKeyInformationDto(newApplicationDto.JobId, newApplicationDto.SeekerId, newApplicationDto.HirerId));
        newApplicationDto.AIRating = aiGivenRating;
        bool created = await applicationRepository.CreateApplicationAsync(newApplicationDto);
        if (!created) return false;
        
        var applicationsCount = await jobRepository.UpdateApplicationsCountAsync(newApplicationDto.JobId);
        
        if (job.ApplicationsLimit - applicationsCount < _remainingApplicationsThresholdForClosingSoon * job.ApplicationsLimit / 100f) {
            await jobRepository.SetJobStatusAsync(newApplicationDto.JobId, JobStatus.ClosingSoon);
        }
        if (job.ApplicationsLimit <= applicationsCount) {
            await jobRepository.SetJobStatusAsync(newApplicationDto.JobId, JobStatus.Closed);
            return false;
        }

        return true;
    }

    public async Task DeleteApplicationAsync(int applicationId) {
        await applicationRepository.DeleteApplicationAsync(applicationId);
    }
}