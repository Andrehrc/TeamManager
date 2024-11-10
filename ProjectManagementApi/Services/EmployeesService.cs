using ProjectManagementApi.Core.Models;
using ProjectManagementApi.Repositories.Interfaces;
using ProjectManagementApi.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ProjectManagementApi.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesService(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<List<Employee>> GetEmployeesAsync(int userId)
        {
            return await _employeesRepository.GetAllEmployeesAsync(userId);
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id, int userId)
        {
            return await _employeesRepository.GetEmployeeByIdAsync(id, userId);
        }

        public async Task<string> CreateEmployeeAsync(Employee employee, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string base64Image = await ConvertImageToBase64Async(imageFile);
                employee.ImagePath = base64Image;
            }

            await _employeesRepository.AddEmployeeAsync(employee);
            return "Employee saved successfully";
        }

        public async Task<string> UpdateEmployeeAsync(Employee employee, IFormFile imageFile, int userId)
        {
            var dbEmployee = await _employeesRepository.GetEmployeeByIdAsync(employee.Id, userId);

            if (imageFile != null && imageFile.Length > 0)
            {
                string base64Image = await ConvertImageToBase64Async(imageFile);
                employee.ImagePath = base64Image;
            }

            await _employeesRepository.UpdateEmployeeAsync(employee);
            return "Employee updated successfully";
        }

        public async Task<string> DeleteEmployeeAsync(Employee employee)
        {
            await _employeesRepository.DeleteEmployeeAsync(employee);
            return "Employee removed successfully";
        }

        private async Task<string> ConvertImageToBase64Async(IFormFile imageFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                using (var image = Image.Load(imageBytes))
                {
                    var encoder = new JpegEncoder
                    {
                        Quality = 75
                    };

                    using (var compressedStream = new MemoryStream())
                    {
                        await image.SaveAsync(compressedStream, encoder);
                        imageBytes = compressedStream.ToArray();
                    }
                }

                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}
