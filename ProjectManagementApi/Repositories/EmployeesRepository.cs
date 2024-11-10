using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Contexts;
using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;
using System;

namespace ProjectManagementApi.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(int userId)
        {
            return await _dbContext.Employees.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id, int userId)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }
    }
}
