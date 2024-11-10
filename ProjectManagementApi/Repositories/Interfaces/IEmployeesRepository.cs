using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Core.Models;

namespace ProjectManagementApi.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync(int userId);
        Task<Employee> GetEmployeeByIdAsync(int id, int userId);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(Employee employee);
    }
}
