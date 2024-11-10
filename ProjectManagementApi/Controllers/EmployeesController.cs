using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Services.Interfaces;

namespace ProjectManagementApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var employees = await _employeesService.GetEmployeesAsync(userId);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var employee = await _employeesService.GetEmployeeByIdAsync(id, userId);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee, [FromForm] IFormFile imageFile)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            employee.UserId = userId;
            var result = await _employeesService.CreateEmployeeAsync(employee, imageFile);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee, [FromForm] IFormFile imageFile)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var result = await _employeesService.UpdateEmployeeAsync(employee, imageFile, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var userIdClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }

            var employee = await _employeesService.GetEmployeeByIdAsync(id, userId);
            if (employee == null)
            {
                return NotFound();
            }

            var result = await _employeesService.DeleteEmployeeAsync(employee);
            return Ok(result);
        }
    }
}
