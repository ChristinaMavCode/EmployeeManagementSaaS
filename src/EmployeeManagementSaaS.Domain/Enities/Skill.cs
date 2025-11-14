namespace EmployeeManagementSaaS.Domain.Entities;

public class Skill
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required DateTime CreationAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}