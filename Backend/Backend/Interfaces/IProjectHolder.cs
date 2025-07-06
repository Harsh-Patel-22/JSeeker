using Backend.DTOs.Users;

namespace Backend.Interfaces;

public interface IProjectHolder {
    public Task<List<ProjectDetailsDto>> GetProjectsAsync(Guid userId);
}