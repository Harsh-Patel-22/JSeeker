using Backend.DTOs.Job;
using Backend.Repositories;

namespace Backend.Services;

public class RatingService(AIService aiService, UserRepository userRepository, JobRepository jobRepository) {
    public async Task<int> GetAIRatingForApplicationAsync(ApplicationKeyInformationDto application) {
        var jobDescription = await jobRepository.GetJobDescriptionByIdAsync(application.JobId);
        var applicantProjectDetails = await userRepository.GetProjectsAsync(application.SeekerId);
        var applicantWorkExperienceDetails = await userRepository.GetWorkExperienceDetailsAsync(application.SeekerId);
        var applicantEducationDetails = await userRepository.GetEducationDetailsAsync(application.SeekerId);

        // string baseQuery = "For the given json, rate on a scale of 1-100, how much is the user really applicable/reliable/worthy for the mentioned job. Provide me with only the rating. I dont need any text or choices. Just give me the number for rating.";
        // string ratingString = await aiService.GetChatResponseAsync($"{baseQuery} Job Details: {jobDescription}, Applicant Projects: {applicantProjectDetails},  Applicant Work Experience: {applicantWorkExperienceDetails}, Applicant Education Details: {applicantEducationDetails}");
        // if (!int.TryParse(ratingString, out int rating)) {
        //     throw new Exception("Invalid rating");
        // }
        // return rating;
        return 5;
    }
}