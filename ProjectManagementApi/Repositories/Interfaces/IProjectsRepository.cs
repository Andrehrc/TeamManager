using ProjectManagementApi.Core.Models;

namespace ProjectManagementApi.Repositories.Interfaces
{
    public interface IProjectsRepository
    {
        Task<List<Project>> GetProjectsAsync(int userId);
        Task<Project> GetProjectByIdAsync(int id, int userId);
        Task AddProjectAsync(Project Project);
        Task UpdateProjectAsync(Project Project);
        Task DeleteProjectAsync(int id);
    }
}
