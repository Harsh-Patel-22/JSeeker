using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Models;
using Backend.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class InterviewRepository (ApplicationDbContext context) {
    
    // TODO - Make the fetching datewise/jobwise
    public async Task<List<InterviewDto>> GetInterviewsByIdByScheduleStatusAsync(Guid userId, bool scheduled) {
        var query = context.Interviews.Include(i => i.Job)
            .ThenInclude(job => job.Address)
            .Where(i => i.HirerId == userId || i.SeekerId == userId);
        query = scheduled ? query.Where(i => i.ConfirmedBySeeker == true && i.ConfirmedByHirer == true) : query.Where(i => i.ConfirmedByHirer == false || i.ConfirmedBySeeker == false);
        var interviews = await query.Select(interview => new InterviewDto() {
            Id = interview.ApplicationId,
            SeekerId = interview.SeekerId,
            HirerId = interview.HirerId,
            JobId = interview.Job.Id,
                    
            FirstName = interview.Seeker.FirstName,
            LastName = interview.Seeker.LastName,
            Email = interview.Seeker.Email,
            PhoneNumber = interview.Seeker.PhoneNumber,
                    
            Date = interview.Date,
            Time = interview.Time,
            Mode = interview.Mode,
            ConfirmedByHirer = interview.ConfirmedByHirer,
            ConfirmedBySeeker = interview.ConfirmedBySeeker
        }).ToListAsync();
        
        return interviews;
    }

    public async Task<bool> CreateInterviewsAsync(CreateInterviewDto newInterviewDto) {
        try {
            await context.Interviews.AddAsync(new Interview() {
                ApplicationId = newInterviewDto.ApplicationId,
                Date = newInterviewDto.DateProposedByHirer,
                Time = newInterviewDto.TimeProposedByHirer,
                Mode = newInterviewDto.Mode,
                HirerId = newInterviewDto.HirerId,
                SeekerId = newInterviewDto.SeekerId,
                JobId = newInterviewDto.JobId,
                
                ConfirmedByHirer = true,
                ConfirmedBySeeker = false
            });
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e) {
            return false;
        }
    }
    
    public async Task UpdateInterviewDateTimeAsync(int interviewId, DateAndTimeDto dto, bool confirmedByHirer) {
        if (confirmedByHirer) {
            await context.Interviews.Where(i => i.ApplicationId == interviewId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(i => i.Date, dto.Date)
                    .SetProperty(i => i.Time, dto.Time)
                    .SetProperty(i => i.ConfirmedByHirer, true)
                    .SetProperty(i => i.ConfirmedBySeeker, false));
        }
        else {
            await context.Interviews.Where(i => i.ApplicationId == interviewId)
                .ExecuteUpdateAsync(setter => setter.SetProperty(i => i.Date, dto.Date)
                    .SetProperty(i => i.Time, dto.Time)
                    .SetProperty(i => i.ConfirmedByHirer, false)
                    .SetProperty(i => i.ConfirmedBySeeker, true));
        }
    }

    public async Task SetScheduledTrueAsync(int interviewId) {
        await context.Interviews.Where(i => i.ApplicationId == interviewId).ExecuteUpdateAsync(setter => setter.SetProperty(i => i.ConfirmedByHirer, true).SetProperty(i => i.ConfirmedBySeeker, true));
    }

    public async Task<bool> GetApprovedStatusAsync(int interviewId, Roles role) {
        switch (role) {
            case Roles.Hirer:
                return await context.Interviews.Where(i => i.ApplicationId == interviewId).Select(i => i.ConfirmedByHirer).FirstOrDefaultAsync();

            case Roles.Seeker:
                return await context.Interviews.Where(i => i.ApplicationId == interviewId).Select(i => i.ConfirmedBySeeker).FirstOrDefaultAsync();
            
        }

        return true;
    }
    
    public async Task<Guid> GetSeekerIdByInterviewId(int interviewId) {
        return await context.Interviews.Where(i => i.ApplicationId == interviewId).Select(i => i.SeekerId).FirstOrDefaultAsync();
    }

    public async Task<InterviewDto> GetInterviewByIdAsync(int interviewId) {
        return (await context.Interviews.Where(i => i.ApplicationId == interviewId).Select(interview => new InterviewDto() {
            Id = interview.ApplicationId,
            SeekerId = interview.SeekerId,
            HirerId = interview.HirerId,
            JobId = interview.Job.Id,
                    
            FirstName = interview.Seeker.FirstName,
            LastName = interview.Seeker.LastName,
            Email = interview.Seeker.Email,
            PhoneNumber = interview.Seeker.PhoneNumber,
                    
            Date = interview.Date,
            Time = interview.Time,
            Mode = interview.Mode,
            ConfirmedByHirer = interview.ConfirmedByHirer,
            ConfirmedBySeeker = interview.ConfirmedBySeeker
        }).FirstOrDefaultAsync())!;
    }
}