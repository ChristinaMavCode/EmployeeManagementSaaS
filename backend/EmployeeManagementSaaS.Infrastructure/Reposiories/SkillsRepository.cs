namespace EmployeeManagementSaaS.Infrastructure.Reposiories
{
    public class SkillsRepository : ISkillsRepository
    {
        public Task<Skill> CreateSkill(Skill request)
        {
            request.Id = Guid.NewGuid();
            EmployeeSkillsContext.Skills.Add(request);
            return Task.FromResult(request);
        }

        public Task<List<Skill>> GetSkills()
        {
            var skills = EmployeeSkillsContext.Skills.ToList();
            return Task.FromResult(skills);
        }

        public Task<bool> SkillNameExistsAsync(string name)
        {
            bool exists = EmployeeSkillsContext.Skills.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task<bool> SkillExistsAsync(string skillID)
        {
            bool exists = EmployeeSkillsContext.Skills.Any(x => x.Id.ToString() == skillID);
            return Task.FromResult(exists);
        }
    }
}
