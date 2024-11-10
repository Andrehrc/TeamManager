using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;

namespace ProjectManagementApi.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _ProjectRepository;

        public ProjectsService(IProjectsRepository ProjectRepository)
        {
            _ProjectRepository = ProjectRepository;
        }

        public async Task<List<Project>> GetProjectsAsync(int userId)
        {
            return await _ProjectRepository.GetProjectsAsync(userId);
        }

        public async Task<Project> GetProjectByIdAsync(int id, int userId)
        {
            return await _ProjectRepository.GetProjectByIdAsync(id, userId);
        }

        public async Task<string> CreateProjectAsync(Project Project)
        {
            await _ProjectRepository.AddProjectAsync(Project);
            return "Project salvo com sucesso";
        }

        public async Task<string> UpdateProjectAsync(Project Project)
        {
            await _ProjectRepository.UpdateProjectAsync(Project);
            return "Project atualizado com sucesso";
        }

        public async Task<string> DeleteProjectAsync(int id)
        {
            await _ProjectRepository.DeleteProjectAsync(id);
            return "Project removido com sucesso";
        }
    }
}
