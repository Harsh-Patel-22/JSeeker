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
    // TODO - Add the null checks (string length 0 and return responses accordingly
    [HttpGet("languages")]
    public async Task<IActionResult> GetAllLanguages() {
        var languages = await  _githubService.GetAllLanguagesAsync();
        return Ok(languages);
    }
    
    [HttpGet("languages/{owner}/{repo}")]
    public async Task<IActionResult> GetRepoLanguages([FromRoute] string owner, [FromRoute] string repo) {
        var languages = await _githubService.GetRepoLanguagesAsync(owner, repo);
        return Ok(languages);
    }
    
    [HttpGet("repositories/{owner}/{repo}")]
    public async Task<IActionResult> GetUserRepo([FromRoute] string owner, [FromRoute] string repo) {
        var repoData = await _githubService.GetRepoAsync(owner, repo);
        return Ok(repoData);
    }
    
    [HttpGet("repositories/{owner}")]
    public async Task<IActionResult> GetUserRepos([FromRoute] string owner) {
        var repos = await _githubService.GetUserReposAsync(owner);
        return Ok(repos);
    }
    
    [HttpGet("topics/{owner}/{repo}")]
    public async Task<IActionResult> GetTopics([FromRoute] string owner, [FromRoute] string repo) {
        var topics = await  _githubService.GetRepoTopicsAsync(owner, repo);
        return Ok(topics);
    }
    
    
    [HttpGet("tags/{owner}/{repo}")]
    public async Task<IActionResult> GetTags([FromRoute] string owner, [FromRoute] string repo) {
        var tags = await  _githubService.GetRepoTagsAsync(owner, repo);
        return Ok(tags);
    }
    
    
    [HttpGet("readme/{owner}/{repo}")]
    public async Task<IActionResult> GetReadme([FromRoute] string owner, [FromRoute] string repo) {
        var readmeString = await  _githubService.GetRepoReadmeAsync(owner, repo);
        return Ok(readmeString);
    }

    [HttpGet("allinsights/repo={repo}")]
    public async Task<IActionResult> GetAllInsights([FromRoute] string repo) {
        var insights = await _githubService.GetAllInsightsFromRepoAsync(repo); 
        return Ok(insights);
    }
}