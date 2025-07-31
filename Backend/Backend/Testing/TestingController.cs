using Backend.DTOs.Job;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Testing;

[ApiController]
[Route("api/testing")]
public class TestingController(ResumeBuilderService resumeBuilder, PdfService pdfService, RatingService ratingService) : ControllerBase {
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
}