using Backend.DTOs.Job;
using Backend.Repositories;

namespace Backend.Services;

public class RatingService(AIService aiService, UserRepository userRepository, JobRepository jobRepository) {
    public async Task<int> GetAIRatingForApplicationAsync(ApplicationKeyInformationDto application) {
        var jobDescription = await jobRepository.GetJobDescriptionByIdAsync(application.JobId);
        var applicantProjectDetails = await userRepository.GetProjectsAsync(application.SeekerId);
        var applicantWorkExperienceDetails = await userRepository.GetWorkExperienceDetailsAsync(application.SeekerId);
        var applicantEducationDetails = await userRepository.GetEducationDetailsAsync(application.SeekerId);

        string baseQuery = "You are a professional technical recruiter and hiring AI.\n\nYour task is to evaluate how well an applicant matches a given job description.\n\nYou will receive the following inputs in JSON format:\n\n1. `jobDescription`: A detailed job description containing role responsibilities, required skills, experience, and qualifications.\n2. `applicantProjectDetails`: A list of the applicant’s past projects including technologies used, responsibilities, outcomes, etc.\n3. `applicantWorkExperienceDetails`: Work experience with roles, durations, achievements, and technologies.\n4. `applicantEducationDetails`: Education background with degrees, institutions, and notable coursework or achievements.\n\n---\n\n### 🎯 Objective:\n\nAnalyze the job description and compare it against the applicant's provided data to determine how well-suited the applicant is for the role.\n\nTake into account:\n- Alignment between the required and demonstrated technical skills.\n- Relevance of past projects and work experience.\n- Depth of domain knowledge.\n- Level of education or certifications.\n- Matching responsibilities and role expectations.\n\n---\n\n### ✅ Output Format:\n\nRespond with **only the following JSON** (no explanations, no markdown):\n\n```json\n{\n  \"matchScore\": 87,\n  \"reason\": \"The applicant demonstrates strong alignment with the job's tech stack and has relevant project experience in web development and cloud platforms. However, there is limited experience in leadership roles which is preferred in the job description.\"\n}\n";
        // string ratingString = await aiService.GetChatResponseAsync($"{baseQuery} Job Details: {jobDescription}, Applicant Projects: {applicantProjectDetails},  Applicant Work Experience: {applicantWorkExperienceDetails}, Applicant Education Details: {applicantEducationDetails}");
        // if (!int.TryParse(ratingString, out int rating)) {
        //     throw new Exception("Invalid rating");
        // }
        // return rating;
        return 5;
    }
}