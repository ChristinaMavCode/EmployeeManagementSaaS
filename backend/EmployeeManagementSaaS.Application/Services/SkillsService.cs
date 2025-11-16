using AutoMapper;
using EmployeeManagementSaaS.Domain.Entities;
using MediatR;

namespace EmployeeManagementSaaS.Application.Products;

public class SkillsService : ISkillsService
{
    private readonly ILogger<SkillsService> _logger;
    private readonly ISkillsRepository _skillsRepository;
    private readonly IMapper _mapper;

    public SkillsService(ILogger<SkillsService> logger, ISkillsRepository skillsRepository, IMapper mapper)
    {        
        _mapper = mapper;
        _logger = logger;
        _skillsRepository = skillsRepository;
    }    

    public async Task<List<SkillDto>> GetSkills()
    {
        try
        {
            var skills = await _skillsRepository.GetSkills().ConfigureAwait(false);
            return skills.Select(skill => _mapper.Map<SkillDto>(skill)).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving skills");
            return new List<SkillDto>();
        }
    }

    public async Task<SkillDto?> CreateSkill(CreateSkillCommand request)
    {
        try
        {
            var skillEntity = _mapper.Map<Skill>(request);
            var createdSkill = await _skillsRepository.CreateSkill(skillEntity).ConfigureAwait(false);
            return _mapper.Map<SkillDto>(createdSkill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill");
            return null;
        }
    }
}