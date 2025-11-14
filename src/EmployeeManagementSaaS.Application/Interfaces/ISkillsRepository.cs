namespace EmployeeManagementSaaS.Application.Interfaces;

public interface ISkillsRepository
{
    Task<List<Skill>> GetSkills();

    Task<Skill?> CreateSkill(Skill request);

    Task<bool> SkillNameExistsAsync(string name);
}