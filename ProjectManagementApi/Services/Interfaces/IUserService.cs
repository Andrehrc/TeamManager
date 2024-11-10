namespace ProjectManagementApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<(string token, string refreshToken)> SignUpAsync(string name, string email, string password);
    }
}
