using EmployeeManagementSaaS.Application.Commands;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Validations;
using FluentValidation.TestHelper;
using Moq;

namespace EmployeeManagementSaaS.UnitTests;

public class AssignSkillToEmployeeCommandValidatorTests
{
    private readonly AssignSkillToEmployeeCommandValidator _validator;

    public AssignSkillToEmployeeCommandValidatorTests()
    {
        // For async uniqueness checks, you can mock the repository
        var mockEmployeeRepo = new Mock<IEmployeesRepository>();
        mockEmployeeRepo.Setup(r => r.EmployeeExistsAsync("1")).ReturnsAsync(true);
        mockEmployeeRepo.Setup(r => r.EmployeeExistsAsync("2")).ReturnsAsync(false);
        mockEmployeeRepo.Setup(r => r.EmployeeAlreadyHasSkillAsync("1", "1")).ReturnsAsync(false);
        mockEmployeeRepo.Setup(r => r.EmployeeAlreadyHasSkillAsync("1", "2")).ReturnsAsync(true);

        var mockSkillsRepo = new Mock<ISkillsRepository>();
        mockSkillsRepo.Setup(r => r.SkillExistsAsync("1")).ReturnsAsync(true);
        mockSkillsRepo.Setup(r => r.SkillExistsAsync("2")).ReturnsAsync(true);
        mockSkillsRepo.Setup(r => r.SkillExistsAsync("3")).ReturnsAsync(false);

        _validator = new AssignSkillToEmployeeCommandValidator(mockEmployeeRepo.Object, mockSkillsRepo.Object);
    }

    [Fact]
    public async Task Should_Have_Error_When_Employee_Not_Exists()
    {
        var command = new AssignSkillToEmployeeCommand { EmployeeID = "2", SkillID = "1" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.EmployeeID);
    }

    [Fact]
    public async Task Should_Have_Error_When_Skill_Not_Exists()
    {
        var command = new AssignSkillToEmployeeCommand { EmployeeID = "1", SkillID = "3" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c.SkillID);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Not_Unique()
    {
        var command = new AssignSkillToEmployeeCommand { EmployeeID = "1", SkillID = "2" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(c => c)
              .WithErrorMessage("Employee Already Has Skill");
    }

    [Fact]
    public async Task Should_Not_Have_Error_For_Valid_Command()
    {
        var command = new AssignSkillToEmployeeCommand { EmployeeID = "1", SkillID = "1" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}