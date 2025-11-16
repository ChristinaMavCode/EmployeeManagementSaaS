using EmployeeManagementSaaS.Application.Commands;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Validations;
using FluentValidation.TestHelper;
using Moq;

namespace EmployeeManagementSaaS.UnitTests;

public class CreateSkillCommandValidatorTests
{
    private readonly CreateSkillCommandValidator _validator;

    public CreateSkillCommandValidatorTests()
    {
        // For async uniqueness checks, you can mock the repository
        var mockRepo = new Mock<ISkillsRepository>();
        mockRepo.Setup(r => r.SkillNameExistsAsync("C#")).ReturnsAsync(true);
        mockRepo.Setup(r => r.SkillNameExistsAsync("Java")).ReturnsAsync(false);

        _validator = new CreateSkillCommandValidator(mockRepo.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateSkillCommand { Name = "", Description = "desc" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        var command = new CreateSkillCommand { Name = "C#", Description = "desc" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.Name)
              .WithErrorMessage("Skill name must be unique");
    }

    [Fact]
    public async Task Should_Not_Have_Error_For_Valid_Command()
    {
        var command = new CreateSkillCommand { Name = "Java", Description = "desc" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}