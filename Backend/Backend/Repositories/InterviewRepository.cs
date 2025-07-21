using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class InterviewRepository (ApplicationDbContext context) {
    
    public async Task<List<InterviewDto>> GetInterviewsByIdAsync(Guid userId) {
        List<InterviewDto> interviews = await context.Interviews.Include(i => i.Job)
            .ThenInclude(job => job.Address)
            .Where(i => i.HirerId == userId || i.SeekerId == userId)
            .Select(interview => new InterviewDto() {
                SeekerId = interview.SeekerId,
                HirerId = interview.HirerId,
                JobId = interview.JobId,
                Job = new JobDto() {
                    Title = interview.Job.Title, 
                    Description = interview.Job.Description,
                    TermsAndConditions = interview.Job.TermsAndConditions,
                    MinSalary = interview.Job.MinSalary, 
                    MaxSalary = interview.Job.MaxSalary, 
                    Address = interview.Job.Address
                } ,
                Date = interview.Date,
                Time = interview.Time,
                Mode = interview.Mode,
            }).ToListAsync();
        
        return interviews;
    }

    public async Task<bool> CreateInterviewsAsync(CreateInterviewDto newInterviewDto) {
        try {
            await context.Interviews.AddAsync(new Interview() {
                Date = newInterviewDto.Date,
                Time = newInterviewDto.Time,
                Mode = newInterviewDto.Mode,
                HirerId = newInterviewDto.HirerId,
                SeekerId = newInterviewDto.SeekerId,
                JobId = newInterviewDto.JobId
            });
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }
}