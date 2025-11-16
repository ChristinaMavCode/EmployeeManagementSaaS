namespace EmployeeManagementSaaS.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Surname { get; set; }

    public List<Skill> Skills { get; set; }    

    public DateTime? CreationAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}