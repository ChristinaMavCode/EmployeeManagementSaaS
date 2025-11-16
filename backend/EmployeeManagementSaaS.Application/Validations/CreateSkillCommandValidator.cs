
namespace EmployeeManagementSaaS.Application.Validations;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    private readonly ISkillsRepository _repository;

    public CreateSkillCommandValidator(ISkillsRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Skill name is required")
            .MaximumLength(20).WithMessage("Skill name must be 20 characters or less")
            .MustAsync(BeUniqueName).WithMessage("Skill name must be unique");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description must be 100 characters or less");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _repository.SkillNameExistsAsync(name);
    }
}