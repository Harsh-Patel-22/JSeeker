using Backend.Data;
using Backend.DTOs;
using Backend.Models.Mapping;
using Backend.Models.Users.WorkRelated;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class ProjectsRepository (ApplicationDbContext context, AIService aiService) {

    private async Task<List<string>> GetAiFoundTechnologies() {
        return null;
    }
    public async Task AddGithubProjectAndMappingsAsync(ProjectTechnologyMappingDto mapping) {
        await context.Projects.AddAsync(mapping.Project);
        await context.SaveChangesAsync();

        List<ProjectTechnology> projectTechnologies = new List<ProjectTechnology>();
        foreach (var technologyNameUsage in mapping.TechnologyUsages) {
            int techId = await GetTechnologyIdAsync(technologyNameUsage.Name);
            
            projectTechnologies.Add(new ProjectTechnology() {
                ProjectId = mapping.Project.Id,
                TechnologyId = techId,
                PercentUsage = technologyNameUsage.Usage,
            });
        }
        
        await context.ProjectTechnologies.AddRangeAsync(projectTechnologies);
        await context.SaveChangesAsync();
    }

    private async Task<int> GetTechnologyIdAsync(string technologyName) {
        var tech = await context.Technologies.Where(technology => technology.Name.Equals(technologyName))
            .FirstOrDefaultAsync();

        if (tech != null) return tech.Id;
        Technology t = new Technology() {
            Name = technologyName
        };
        await context.Technologies.AddAsync(t);
        await context.SaveChangesAsync();
        return t.Id;
    }}