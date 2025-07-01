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
    // TODO - Add the null checks (string length 0 and return responses accordingly
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
        return Ok(readmeString);
    }
}