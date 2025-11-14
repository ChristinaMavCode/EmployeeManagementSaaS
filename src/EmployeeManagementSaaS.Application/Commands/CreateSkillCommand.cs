
namespace EmployeeManagementSaaS.Application.Commands
{
    public class CreateSkillCommand : IRequest<SkillDto>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CreateSkillCommandHandler : IRequestHandler<CreateSkillCommand, SkillDto>
    {
        private readonly ISkillsService _skillsService;

        public CreateSkillCommandHandler(ISkillsService skillsService)
        {
            _skillsService = skillsService;
        }

        public async Task<SkillDto?> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            return await _skillsService.CreateSkill(request);
        }
    }
}
