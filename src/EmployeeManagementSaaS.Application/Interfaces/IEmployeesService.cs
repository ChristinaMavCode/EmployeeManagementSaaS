namespace EmployeeManagementSaaS.Application.Interfaces;
public interface IEmployeesService
{
    Task<EmployeeDto?> AssignSkillToEmployee(AssignSkillToEmployeeCommand request);
    Task<List<EmployeeDto>> GetEmployees();
}