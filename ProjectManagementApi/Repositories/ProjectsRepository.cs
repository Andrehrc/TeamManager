using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Contexts;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;

namespace ProjectManagementApi.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Project>> GetProjectsAsync(int userId)
        {
            return await _dbContext.Projects.Where(w => w.UserId == userId)
                                            .Include(x => x.Stages)
                                            .AsNoTracking()
                                            .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id, int userId)
        {
            return await _dbContext.Projects.Where(w => w.UserId == userId)
                                            .Include(x => x.Stages)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddProjectAsync(Project Project)
        {
            await _dbContext.Projects.AddAsync(Project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project Project)
        {
            var stagesToRemove = Project.Stages.Where(w => w.Delete).ToList();
            if (stagesToRemove.Count > 0)
            {
                foreach (var stage in stagesToRemove)
                {
                    _dbContext.DevelopmentStages.Remove(stage);
                }
            }

            _dbContext.Projects.Update(Project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var Project = await _dbContext.Projects.FindAsync(id);
            if (Project != null)
            {
                _dbContext.Projects.Remove(Project);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
