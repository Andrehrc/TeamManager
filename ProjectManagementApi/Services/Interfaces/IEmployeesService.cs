using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories;

namespace ProjectManagementApi.Services.Interfaces
{
    public interface IEmployeesService
    {
        Task<List<Employee>> GetEmployeesAsync(int userId);
        Task<Employee> GetEmployeeByIdAsync(int id, int userId);
        Task<string> CreateEmployeeAsync(Employee employee, IFormFile imageFile);
        Task<string> UpdateEmployeeAsync(Employee employee, IFormFile imageFile, int userId);
        Task<string> DeleteEmployeeAsync(Employee employee);
    }
}
