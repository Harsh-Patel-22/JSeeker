using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class JobService (JobRepository repository) {
    
    public List<JobCardDto> GetRelevantJobs(int clientId) {
        return repository.GetRelevantJobs(clientId);
    }

    public async Task<bool> CreateJobAsync(CreateJobDto newJob) {
        return await repository.CreateJobAsync(newJob); 
    }
}