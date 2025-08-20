using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Exceptions;
using Backend.Models;
using Backend.Models.Users;
using Backend.Repositories;
using Backend.Services.Query;

namespace Backend.Services;

public class JobService (JobRepository jobRepository, InterviewRepository interviewRepository, ApplicationRepository applicationRepository, UserRepository userRepository, RatingService ratingService, ValidationService validationService, JobsAggregateQueryService jobsQueryService) {
    
    private readonly int _remainingApplicationsThresholdForClosingSoon = 10; 
    // Section - Job Related

   
    public async Task<List<JobCardDto>?> GetRelevantJobsAsync(Guid userId, Roles role, JobSearchFilterDto searchFilter) {
        if (!await validationService.UserExistsAsync(userId))
            throw new Exception("No such user exists.");
        if (role == Roles.Hirer && !await validationService.IsHirerAsync(userId)) {
            throw new Exception("Wrong role provided.");
        }
        return await jobRepository.GetRelevantJobsBySearchFilterAsync(userId, role, searchFilter);
    }
    
    public async Task<bool> CreateJobAsync(Guid hirerId, CreateJobDto newJob) {
        if (!await validationService.IsHirerAsync(hirerId))
            throw new Exception("Unauthorized");
        
        return await jobRepository.CreateJobAsync(hirerId, newJob); 
    }

    public async Task<bool> UpdateJobAsync(Guid hirerId, int jobId, EditJobDto dto) {
        if (!await validationService.IsHirerAsync(hirerId)) {
            throw new Exception("Unauthorized");
        }

        if (!await validationService.IsTheirJobAsync(hirerId, jobId)) {
            throw new Exception("Unauthorized");
        }
        
        return await jobRepository.EditJobAsync(jobId, dto);
    }

    public async Task<List<JobForMapMarkerDto>?> GetNearbyJobs(Guid userId, Roles role, decimal searchRadius, JobSearchFilterDto searchFilter) {
        if (!await validationService.UserExistsAsync(userId))
            throw new Exception("No such user exists.");
        if (role == Roles.Hirer && !await validationService.IsHirerAsync(userId)) {
            throw new Exception("Wrong role provided.");
        }
        return await jobRepository.GetNearbyJobsAsync(userId, role, searchRadius, searchFilter);
    }

    public async Task<JobDescriptionDto> GetJobDescriptionByIdAsync(int id) {
        var jobDescription = await jobRepository.GetJobDescriptionByIdAsync(id);
        if(jobDescription == null) 
            throw new Exception("Job Does Not Exist");
        return jobDescription;
    }
    
    // Section - Interview Related
    public async Task<List<InterviewDto>> GetInterviewsByIdByScheduleStatusAsync(Guid userId, bool scheduled) {
        if (!await validationService.UserExistsAsync(userId))
            throw new Exception("No such user Exist");
        return await interviewRepository.GetInterviewsByIdByScheduleStatusAsync(userId, scheduled);
    }

    public async Task<InterviewDto> GetInterviewByIdAsync(Guid userId, int interviewId) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new Exception("No such user exists");
        }

        if (!await validationService.InterviewExistsAsync(interviewId)) {
            throw new Exception("Interview Does Not Exist");
        }
        
        if (!await validationService.IsTheirInterviewAsync(userId, interviewId)) {
            throw new Exception("Unauthorized");
        }
        var interviewDto = await interviewRepository.GetInterviewByIdAsync(interviewId);
        return interviewDto;
    }
    
    public async Task UpdateInterviewDateTimeAsync(Guid userId, int interviewId, Roles role, DateAndTimeDto dto) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new Exception("No such user exists");
        }

        if (!await validationService.IsTheirInterviewAsync(userId, interviewId)) {
            throw new Exception("Unauthorized");
        }

        switch (role) {
            case Roles.Hirer:
                await interviewRepository.UpdateInterviewDateTimeAsync(interviewId, dto, true);
                break;
            case Roles.Seeker:
                await interviewRepository.UpdateInterviewDateTimeAsync(interviewId, dto, false);
                break;
        }
        
    }
    
    public async Task<bool> CreateInterviewAsync(CreateInterviewDto newInterviewDto) {
        if (!await validationService.UserExistsAsync(newInterviewDto.SeekerId)) {
            throw new Exception("No such seeker exists");
        }

        if (!await validationService.IsHirerAsync(newInterviewDto.HirerId)) {
            throw new Exception("Unauthorized");
        }

        if (!await validationService.JobExistsAsync(newInterviewDto.JobId)) {
            throw new Exception("No such job exists");
        }
        // TODO --
        // await UpdateApplicationStateAsync(newInterviewDto.HirerId, new ApplicationStateUpdateDto(newInterviewDto.))
        return await interviewRepository.CreateInterviewsAsync(newInterviewDto);
    }

    public async Task SetInterviewScheduledAsync(Guid userId, Roles role, int applicationId) {
        if (!await interviewRepository.GetApprovedStatusAsync(applicationId, role)) {
            await interviewRepository.SetScheduledTrueAsync(applicationId);
            await UpdateApplicationStateAsync(userId, new ApplicationStateUpdateDto(applicationId, ApplicationState.InterviewScheduled));
        }
        else {
            throw new GlobalExceptions.Unauthorised();
        }
    }
    
    public async Task UpdateSeekerSuccessFailureJobLandingAsync(Guid hirerId, int interviewId, bool successful) {
        if (!await validationService.InterviewExistsAsync(interviewId)) {
            throw new Exception("No such interview exists");
        }

        if (!await validationService.IsTheirInterviewAsync(hirerId, interviewId)) {
            throw new Exception("Unauthorized");
        }
        
        Guid userId = await interviewRepository.GetSeekerIdByInterviewId(interviewId);
        if (successful) {
            await userRepository.IncrementSuccessCountAsync(userId);
        }
        else {
            await userRepository.IncrementRejectedCountAsync(userId);
        }
    }
    
    // Section - Application Related
    public async Task<List<ApplicationDto>> GetAllApplicationsByUserIdByStateAsync(Guid userId, ApplicationState state) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }
        return await applicationRepository.GetAllApplicationsByUserIdByStateAsync(userId, state);
    }

    public async Task<Dictionary<int, List<ApplicationDto>>> GetAllApplicationsByUserIdJobWiseAsync(Guid userId) {
        if (!await validationService.IsHirerAsync(userId)) {
            throw new GlobalExceptions.Unauthorised();
        }

        return await jobsQueryService.GetAllApplicationsByUserIdJobWiseAsync(userId);
    }

    public async Task<ApplicationDto> GetApplicationByIdAsync(Guid userId, int applicationId) {
        if (!await validationService.UserExistsAsync(userId)) {
            throw new Exception("No such user exists");
        }

        if (!await validationService.ApplicationExistsAsync(applicationId)) {
            throw new Exception("No such application exists");
        }

        if (!await validationService.IsTheirApplicationAsync(userId, applicationId)) {
            throw new Exception("Unauthorized");
        }
        
        var applicationDto = await applicationRepository.GetApplicationByIdAsync(applicationId);
        return applicationDto;
    }

    public async Task UpdateApplicationStateAsync(Guid hirerId, ApplicationStateUpdateDto dto) {
        if (!await validationService.IsHirerAsync(hirerId)) {
            throw new Exception("Unauthorized");
        }

        if (!await validationService.ApplicationExistsAsync(dto.ApplicationId)) {
            throw new Exception("No such application exists");
        }
        
        if (!await validationService.IsTheirApplicationAsync(hirerId, dto.ApplicationId)) {
            throw new Exception("Unauthorized");
        }
        
       
        await applicationRepository.UpdateApplicationStateAsync(dto.ApplicationId, dto.State);
        if (dto.State == ApplicationState.Rejected) {
            Guid userId = await applicationRepository.GetSeekerIdByApplicationIdAsync(dto.ApplicationId);
            await userRepository.IncrementRejectedCountAsync(userId);
        }
        
    }

    
    public async Task<bool> CheckAndCreateApplicationAsync(CreateApplicationDto newApplicationDto) {
        if (!await validationService.JobExistsAsync(newApplicationDto.JobId)) {
            throw new Exception("No such job exists");
        }
        if (!await validationService.UserExistsAsync(newApplicationDto.SeekerId)) {
            throw new Exception("No such seeker exists");
        }

        if (!await validationService.IsHirerAsync(newApplicationDto.HirerId)) {
            throw new Exception("No such hirer exists");
        }
        
        
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

    public async Task DeleteApplicationAsync(Guid userId, int applicationId) {
        if (!await validationService.ApplicationExistsAsync(applicationId)) {
            throw new Exception("No such application exists");
        }

        if (!await validationService.IsTheirApplicationAsync(userId, applicationId)) {
            throw new Exception("Unauthorized");
        }
        
        await applicationRepository.DeleteApplicationAsync(applicationId);
    }
}