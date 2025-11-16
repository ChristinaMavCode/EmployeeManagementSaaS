namespace EmployeeManagementSaaS.Application.Interfaces;

public interface IEmployeesRepository
{
    Task<Employee> AssignSkillToEmployee(string employeeID, string skillID);
    Task<bool> EmployeeExistsAsync(string employeeID);
    Task<bool> EmployeeAlreadyHasSkillAsync(string employeeID, string skillID);
    Task<List<Employee>> GetEmployees();
}
