using ProjectManagementApi.Core.Models;

namespace ProjectManagementApi.Services.Interfaces
{
    public interface IProjectsService
    {
        Task<List<Project>> GetProjectsAsync(int userId);

        Task<Project> GetProjectByIdAsync(int id, int userId);

        Task<string> CreateProjectAsync(Project Project);

        Task<string> UpdateProjectAsync(Project Project);

        Task<string> DeleteProjectAsync(int id);
    }
}
