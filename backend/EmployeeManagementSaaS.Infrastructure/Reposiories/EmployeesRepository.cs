namespace EmployeeManagementSaaS.Infrastructure.Reposiories;

public class EmployeesRepository : IEmployeesRepository
{
    public Task<List<Employee>> GetEmployees()
    {
        var employees = EmployeeSkillsContext.Employees.ToList();
        return Task.FromResult(employees);
    }

    public Task<Employee> AssignSkillToEmployee(string employeeID, string skillID)
    {
        var employee = EmployeeSkillsContext.Employees.FirstOrDefault(x => x.Id.ToString() == employeeID);
        var skill = EmployeeSkillsContext.Skills.FirstOrDefault(x => x.Id.ToString() == skillID);
        employee.Skills.Add(skill);
        return Task.FromResult(employee);
    }

    public Task<bool> EmployeeExistsAsync(string employeeID)
    {
        bool exists = EmployeeSkillsContext.Employees.Any(x => x.Id.ToString() == employeeID);
        return Task.FromResult(exists);
    }

    public Task<bool> EmployeeAlreadyHasSkillAsync(string employeeID, string skillID)
    {
        bool hasSkill = EmployeeSkillsContext.Employees.Any(x => x.Id.ToString() == employeeID && x.Skills.Any(y => y.Id.ToString() == skillID));
        return Task.FromResult(hasSkill);
    }
}
