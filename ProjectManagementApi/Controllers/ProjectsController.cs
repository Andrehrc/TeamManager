using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Services.Interfaces;

namespace ProjectManagementApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _service;

        public ProjectsController(IProjectsService projectsService)
        {
            _service = projectsService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var Projects = await _service.GetProjectsAsync(userId);
            return Ok(Projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var Project = await _service.GetProjectByIdAsync(id, userId);

            if (Project == null)
            {
                return NotFound();
            }

            return Ok(Project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project Project)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            Project.UserId = userId;

            var result = await _service.CreateProjectAsync(Project);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project Project)
        {
            if (id != Project.Id)
            {
                return BadRequest("ID do Project não coincide.");
            }

            var result = await _service.UpdateProjectAsync(Project);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var Project = await _service.GetProjectByIdAsync(id, userId);

            if (Project == null)
            {
                return NotFound("Project não encontrado.");
            }

            var result = await _service.DeleteProjectAsync(Project.Id);
            return Ok(result);
        }
    }
}
