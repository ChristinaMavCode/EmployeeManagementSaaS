namespace EmployeeManagementSaaS.Application.Queries;

public record GetAllSkillsQuery() : IRequest<IReadOnlyCollection<SkillDto>>;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllSkillsQuery, IReadOnlyCollection<SkillDto>>
{
    private readonly ISkillsService _skillsService;

    public GetAllProductsQueryHandler(ISkillsService skillsService)
    {
        _skillsService = skillsService;
    }

    public async Task<IReadOnlyCollection<SkillDto>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
        return await _skillsService.GetSkills();
    }
}
