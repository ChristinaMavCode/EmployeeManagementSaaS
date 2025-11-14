namespace EmployeeManagementSaaS.Application.Interfaces;

public interface ISkillsService
{
    Task<List<SkillDto>> GetSkills();

    Task<SkillDto?> CreateSkill(CreateSkillCommand request);
}