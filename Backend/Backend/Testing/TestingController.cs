using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Testing;

[ApiController]
[Route("api/testing")]
public class TestingController(ResumeBuilderService resumeBuilder, PdfService pdfService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> Get() {
        Guid userId = Guid.NewGuid();
        // var response = await resumeBuilder.GetResumeDescriptionAsync();
        var response = await resumeBuilder.GetFullResumeAsync(userId);
        // var htmlContent = await System.IO.File.ReadAllTextAsync("./../Backend/PdfHtmlTemplates/sample_style.html");
        // htmlContent.Replace("")
        // var pdfBytes = await pdfService.GeneratePdfAsync(await System.IO.File.ReadAllTextAsync("./../Backend/PdfHtmlTemplates/sample_style.html"));
        // return File(pdfBytes, "application/pdf", "resume.pdf");
        return Ok(response);
    }
}