using Microsoft.Playwright;

namespace Backend.Services;

public class PdfService {
    private static bool _playwrightInstalled = false;
    public async Task<byte[]> GeneratePdfAsync(string htmlContent) {
        // if (!_playwrightInstalled) {
        //     await Playwright;
        // }
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions() {
            Headless = true
        });
        
        var page = await browser.NewPageAsync();
        await page.SetContentAsync(htmlContent, new PageSetContentOptions(){WaitUntil = WaitUntilState.NetworkIdle});
        var pdf = await page.PdfAsync(new PagePdfOptions() {
            Format = "A4",
            PrintBackground = true
        });
        return pdf;
    }
}