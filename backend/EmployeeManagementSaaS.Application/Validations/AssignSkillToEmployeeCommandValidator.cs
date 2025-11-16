using FluentValidation;

namespace EmployeeManagementSaaS.Application.Validations;

public class AssignSkillToEmployeeCommandValidator : AbstractValidator<AssignSkillToEmployeeCommand>
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ISkillsRepository _skillsRepository;

    public AssignSkillToEmployeeCommandValidator(IEmployeesRepository employeesRepository, ISkillsRepository skillsRepository)
    {
        _employeesRepository = employeesRepository;
        _skillsRepository = skillsRepository;

        RuleFor(x => x.SkillID)
            .MustAsync(SkillExists).WithMessage("Skill doesn't exist");

        RuleFor(x => x.EmployeeID)
            .MustAsync(EmployeeExists).WithMessage("Employee doesn't exist");

        RuleFor(x => x)
            .MustAsync(EmployeeAlreadyHasSkillAsync).WithMessage("Employee Already Has Skill");
    }

    private async Task<bool> EmployeeExists(string id, CancellationToken cancellationToken)
    {
        return await _employeesRepository.EmployeeExistsAsync(id);
    }

    private async Task<bool> SkillExists(string id, CancellationToken cancellationToken)
    {
        return await _skillsRepository.SkillExistsAsync(id);
    }

    private async Task<bool> EmployeeAlreadyHasSkillAsync(AssignSkillToEmployeeCommand request, CancellationToken cancellationToken)
    {
        return !await _employeesRepository.EmployeeAlreadyHasSkillAsync(request.EmployeeID, request.SkillID);
    }
}