namespace EmployeeManagementSaaS.Application.Dtos;

public sealed class SkillDto
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }
}