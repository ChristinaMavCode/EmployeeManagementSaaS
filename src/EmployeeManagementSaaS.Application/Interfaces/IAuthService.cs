namespace EmployeeManagementSaaS.Application.Interfaces;

public interface IAuthService
{
    Task<string?> Login(LoginDto login);
}
