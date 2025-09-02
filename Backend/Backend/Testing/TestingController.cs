using System.Text.Json;
using Backend.DTOs;
using Backend.DTOs.Job;
using Backend.Extensions;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Testing;

[ApiController]
[Route("api/testing")]
public class TestingController(ResumeBuilderService resumeBuilder, UserRepository userRepository,GithubService githubService, PdfService pdfService, RatingService ratingService, UserService userService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> Get() {
        // Guid userId = Guid.NewGuid();
        // var response = await resumeBuilder.GetResumeDescriptionAsync();
        // var response = await resumeBuilder.GetFullResumeAsync(userId);
        // var htmlContent = await System.IO.File.ReadAllTextAsync("./../Backend/PdfHtmlTemplates/sample_style.html");
        // htmlContent.Replace("")
        // var pdfBytes = await pdfService.GeneratePdfAsync(await System.IO.File.ReadAllTextAsync("./../Backend/PdfHtmlTemplates/sample_style.html"));
        // return File(pdfBytes, "application/pdf", "resume.pdf");
        // return Ok(response);

        // var response = await ratingService.GetAIRatingForApplicationAsync(new ApplicationKeyInformationDto() {
        //     JobId = 10,
        //     // SeekerId = Guid.ParseExact("DEEFA26C-779D-4266-A37D-4E763FB6F660", "string");
        //     
        // });
        return Ok();
    }
    
    [HttpGet("get/resume/pdf")]
    public async Task<IActionResult> GetResumePDFAsync() {
        Guid.TryParse("81675392-9C1E-4A67-B5AC-08A92860B149",  out var userId);
        var resumeContents = await userService.GetResumeTemplateAndStringAsync(userId);
        var pdf = await pdfService.GeneratePdfAsync(resumeContents.ResumeString, resumeContents.TemplateNumber);
        return File(pdf, "application/pdf", "resume.pdf");
    }
    
    [HttpGet("projects")]
    public async Task<IActionResult> GetProjects() {
        var repoNames = await userService.GetAllProjectNamesAsync(Guid.Parse("FB85D0DC-9AAE-4A90-8F9C-67B9ADF32C4E"));
        var limitedRepoNames = new List<string>();
        for (int i = 0; i < 3; i++) {
            limitedRepoNames.Add(repoNames[i]);
        }
        // await userService.UpdateProjectsUsingGithubUsernameAsync(Guid.Parse("FB85D0DC-9AAE-4A90-8F9C-67B9ADF32C4E"), limitedRepoNames);
        return Ok();
    }

    [HttpPost("AIGenKeywords")]
    public async Task<IActionResult> GetAIGenKeywords() {
        Guid userId = User.GetNameIdentifier();
        List<JsonElement> allInsightsList = new  List<JsonElement>(); 
        var repoNames = await githubService.GetTop3RepoNamesAsync(await userRepository.GetGithubUsernameAsync(userId));
        foreach (var repoName in repoNames) {
            var allInsights = await githubService.GetAllInsightsFromRepoAsync(await userRepository.GetGithubUsernameAsync(userId), repoName);
            allInsightsList.Add(allInsights);
        }
        var keywords = await resumeBuilder.GetAIGeneratedKeywordsAsync(allInsightsList);
        return Ok(keywords);
    }
}