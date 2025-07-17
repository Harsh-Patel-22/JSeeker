using System.Text;
using System.Text.Json;
using Backend.DTOs.Users;
using Backend.Models.Users.Cocurricular;
using Backend.Repositories;

namespace Backend.Services;

public class ResumeBuilderService (AIService aiService, GithubService githubService, UserRepository userRepository) {
    private static readonly string BasePromptRegardingJson = "Take the following json. I want my output in the content-type: application/json format as well. Also, don't give me multiple options to select your responses from. Give me a single json back.";

    public async Task<JsonElement> GetFullResumeAsync(Guid userId) {
        BasicDetailsDto? basicDetails = await userRepository.GetBasicDetailsAsync(userId);
        // List<ProjectDetailsDto> projectDetails = await userRepository.GetProjectsAsync(userId); - Try this first. Fast access from local db
        // OR
        var projectDetails = await GetProjectsDetailsAsync();
        ContactDetailsDto? contactDetails = await userRepository.GetContactDetailsAsync(userId);
        List<WorkExperienceDetailsDto> workExperienceDetails = await userRepository.GetWorkExperienceDetailsAsync(userId);
        List<HobbyDto> hobbies = await userRepository.GetHobbiesAsync(userId);
        List<LanguageDto> vocalLanguages = await userRepository.GetVocalLanguagesAsync(userId);
        List<EducationDetailsDto> educationDetails = await userRepository.GetEducationDetailsAsync(userId);
        
        var detailsJsonString = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(basicDetails + projectDetails.ToString() + educationDetails + contactDetails + workExperienceDetails  + hobbies + vocalLanguages));
        
        var response = await aiService.GetChatResponseAsync($"{BasePromptRegardingJson}. You are an expert technical resume writer and software engineer with hiring experience.\n\nI will provide a JSON input that includes my project details, technologies used, toolchain, personal information, and development metadata. Based on this, generate a **complete, job-ready resume in JSON format** — with absolutely no missing or placeholder fields.\n\n### 🎯 Your Tasks:\n1. **Analyze and understand** my input data — projects, configs, tools, tech stack, skills, responsibilities, etc.\n2. **Infer everything** required to create a professional resume. Do NOT leave any fields for me to fill in manually.\n3. Generate all content in a **human-written, polished tone**, optimized for job applications in software development roles.\n4. Use common resume structures and sections for junior/mid-level developers.\n\n---\n\n### ✅ Output JSON Format\n\n```json\n{{\n  \"basics\": {{\n    \"name\": \"Full Name\",\n    \"location\": \"City, Country\",\n    \"email\": \"your@email.com\",\n    \"linkedIn\": \"https://linkedin.com/in/username\",\n    \"github\": \"https://github.com/username\",\n    \"summary\": \"A brief, high-impact summary about who I am as a developer, technologies used, and my strengths.\",\n    \"objective\": \"A 1–2 sentence tailored job objective based on my profile and experience.\"\n  }},\n  \"skills\": {{\n    \"languages\": [\"C#\", \"JavaScript\"],\n    \"frameworks\": [\"ASP.NET Core\", \"React\"],\n    \"tools\": [\"Docker\", \"Postman\", \"Figma\", \"Git\"],\n    \"databases\": [\"PostgreSQL\", \"SQL Server\"],\n    \"devOps\": [\"GitHub Actions\", \"Render\"],\n    \"other\": [\"JWT Auth\", \"REST APIs\", \"LINQ\", \"Clean Architecture\"],\n    \"softSkills\": [\"Problem-solving\", \"Communication\", \"Self-learning\", \"Attention to detail\", \"Teamwork\"]\n  }},\n  \"education\": [\n    {{\n      \"degree\": \"B.Tech in Computer Science and Engineering\",\n      \"institution\": \"Your University Name\",\n      \"location\": \"City, Country\",\n      \"startYear\": 2021,\n      \"endYear\": 2025,\n      \"highlights\": [\n        \"CGPA: 8.4/10\",\n        \"Completed projects in web development and game design\",\n        \"Actively participated in hackathons and developer communities\"\n      ]\n    }}\n  ],\n  \"experience\": [\n    {{\n      \"title\": \"Freelance Developer\",\n      \"organization\": \"Self-employed\",\n      \"location\": \"Remote\",\n      \"startDate\": \"2023-06\",\n      \"endDate\": \"Present\",\n      \"responsibilities\": [\n        \"Designed and developed multiple client-facing web applications using ASP.NET Core and React.\",\n        \"Collaborated with designers and backend teams to deliver high-quality software under tight deadlines.\",\n        \"Built scalable authentication systems using JWT and implemented secure REST APIs.\"\n      ]\n    }}\n  ],\n  \"resumeProjects\": [\n    {{\n      \"title\": \"Project Title\",\n      \"technologies\": [\"ASP.NET Core\", \"React\", \"PostgreSQL\"],\n      \"summary\": \"A 1-line summary of what the project does.\",\n      \"bulletPoints\": [\n        \"Designed a full-stack application using ASP.NET Core and React with a PostgreSQL backend.\",\n        \"Implemented secure authentication, user dashboards, and CI/CD deployment pipelines.\",\n        \"Used GitHub Actions for auto deployment and Tailwind CSS for clean UI.\"\n      ],\n      \"projectLink\": \"https://github.com/username/project\"\n    }}\n  ]\n}}\n  Rules:\nDo not leave any field blank — infer and complete every section.\n\nReturn only valid JSON (no code fences, no markdown, no headings).\n\nAll writing should be clear, confident, and job-ready.\n\nUse concise language with an emphasis on action, results, and skills.\n\nWait for me to send the actual JSON input.\n\nYou will receive a JSON input with:\n\nPersonal Info\n\nTechnologies\n\nTools\n\nProjects (with config & highlights)\n\nUse this to generate the output.\n\nNow wait for me to send the data. If the data is empty, add random data according to you." + $"{detailsJsonString}" /* add json input data here */);
        var cleanedJson = GetCleanJsonString(response);
        return JsonSerializer.Deserialize<JsonElement>(cleanedJson);
    }
    
    public async Task<JsonElement> GetResumeDescriptionAsync() {
        var projectDetails = await GetProjectsDetailsAsync();
        var response = await aiService.GetChatResponseAsync($"{BasePromptRegardingJson}. Here's the json that I have regarding my projects: {projectDetails}. I want the output to have a resume description about me including the technologies and framework I used, regarding other things like some design patterns and architectures used. Give me all work related things from the projects json given above.");
        var cleanedResponse = GetCleanJsonString(response);
        return JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
    }

    public async Task<JsonElement> GetProjectsDetailsAsync() {
        // TKey - project name, TValue - json element with all the necessary details.
        var projectDetails = await githubService.GetAllProjects();
        var response = await aiService.GetChatResponseAsync(
            $"{BasePromptRegardingJson} Give me a project description for the projects. Pick few that you think are the best and give me descriptions. It is for my resume so write accordingly to seek attention and rank higher. Consider this format for json: {{ project name: ..., project description: ..., and others so on.}}. Make sure to not miss out on any key details. {projectDetails} This is the details which I have. ");
        var cleanedResponse = GetCleanJsonString(response);
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
        return jsonElement;
    }

    private string GetCleanJsonString(string json) {
        var cleanedString = json.Replace("```json", "").Replace("```", "").Replace("\n", "").Trim();
        int firstBrace = cleanedString.IndexOf('{');
        if (firstBrace >= 0)
            cleanedString = cleanedString.Substring(firstBrace);

        return cleanedString;
        
    }
}