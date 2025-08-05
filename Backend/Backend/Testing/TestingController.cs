using Backend.DTOs.Job;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Testing;

[ApiController]
[Route("api/testing")]
public class TestingController(ResumeBuilderService resumeBuilder, PdfService pdfService, RatingService ratingService, UserService userService) : ControllerBase {
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
    
    [HttpGet("projects")]
    public async Task<IActionResult> GetProjects() {
        string githubUsername = "Harsh-Patel-22";
        var repoNames = await userService.GetAllProjectNamesAsync(githubUsername);
        var limitedRepoNames = new List<string>();
        for (int i = 0; i < 3; i++) {
            limitedRepoNames.Add(repoNames[i]);
        }
        await userService.UpdateProjectsUsingGithubUsernameAsync(Guid.Parse("FB85D0DC-9AAE-4A90-8F9C-67B9ADF32C4E"), githubUsername, limitedRepoNames);
        return Ok();
    }
}