
namespace EmployeeManagementSaaS.Application.Dtos;

public sealed class EmployeeDto
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public required string Surname { get; set; }

    public string FullName
    {
        get
        {
            return $"{Name} {Surname}";
        }
    }

    public List<SkillDto> Skills { get; set; }
}