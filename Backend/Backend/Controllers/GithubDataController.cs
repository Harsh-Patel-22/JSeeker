using System.Text;
using System.Text.Json;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/github")]
public class GithubDataController : ControllerBase {
    private readonly GithubService _githubService;

    public GithubDataController(GithubService githubService) {
        _githubService = githubService;
    }
    
    [HttpGet("languages")]
    public async Task<IActionResult> GetAllLanguages() {
        var languages = await  _githubService.GetAllLanguages();
        return Ok(languages);
    }
    
    [HttpGet("languages/{owner}/{repo}")]
    public async Task<IActionResult> GetRepoLanguages([FromRoute] string owner, [FromRoute] string repo) {
        var languages = await _githubService.GetRepoLanguages(owner, repo);
        return Ok(languages);
    }
    
    [HttpGet("repositories/{owner}/{repo}")]
    public async Task<IActionResult> GetUserRepo([FromRoute] string owner, [FromRoute] string repo) {
        var repoData = await _githubService.GetRepo(owner, repo);
        return Ok(repoData);
    }
    
    [HttpGet("repositories/{owner}")]
    public async Task<IActionResult> GetUserRepos([FromRoute] string owner) {
        var repos = await _githubService.GetUserRepos(owner);
        return Ok(repos);
    }
    
    [HttpGet("topics/{owner}/{repo}")]
    public async Task<IActionResult> GetTopics([FromRoute] string owner, [FromRoute] string repo) {
        var topics = await  _githubService.GetRepoTopics(owner, repo);
        return Ok(topics);
    }
    
    
    [HttpGet("tags/{owner}/{repo}")]
    public async Task<IActionResult> GetTags([FromRoute] string owner, [FromRoute] string repo) {
        var tags = await  _githubService.GetRepoTags(owner, repo);
        return Ok(tags);
    }
    
    
    [HttpGet("readme/{owner}/{repo}")]
    public async Task<IActionResult> GetReadme([FromRoute] string owner, [FromRoute] string repo) {
        var readmeString = await  _githubService.GetRepoReadme(owner, repo);
        using var doc = JsonDocument.Parse(readmeString);
        var root = doc.RootElement;

        if (!root.TryGetProperty("content", out var contentElement))
            throw new Exception("Content field not found in GitHub response.");

        var base64Content = contentElement.GetString();

        if (string.IsNullOrWhiteSpace(base64Content))
            return Ok(string.Empty);

        // Remove any line breaks in base64 string (GitHub adds \n every 76 chars)
        base64Content = base64Content.Replace("\n", "").Replace("\r", "");

        var bytes = Convert.FromBase64String(base64Content);
        var decodedString = Encoding.UTF8.GetString(bytes);

        return Ok(decodedString);
    }
}