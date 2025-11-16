using AutoMapper;
using EmployeeManagementSaaS.Application;
using EmployeeManagementSaaS.Application.Commands;
using EmployeeManagementSaaS.Application.Dtos;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Products;
using EmployeeManagementSaaS.Application.Validations;
using EmployeeManagementSaaS.Domain.Entities;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagementSaaS.UnitTests;

public class SkillsServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ISkillsRepository> _mockRepo;

    public SkillsServiceTests()
    {

        // AutoMapper configuration for tests
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Skill, SkillDto>();
        });
        _mapper = config.CreateMapper();

        _mockRepo = new Mock<ISkillsRepository>();
    }

    [Fact]
    public async Task GetSkillsAsync_ReturnsMappedDtos()
    {
        // Arrange
        var skills = new List<Skill>
        {
            new Skill { Id = new Guid(), Name = "C#", Description = "Programming", CreationAt = DateTime.Now },
            new Skill { Id = new Guid(), Name = "SQL", Description = "Database", CreationAt = DateTime.Now }
        };
        _mockRepo.Setup(r => r.GetSkills()).ReturnsAsync(skills);

        var service = new SkillsService(Mock.Of<ILogger<SkillsService>>(), _mockRepo.Object, _mapper);

        // Act
        var result = await service.GetSkills();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("C#", result[0].Name);
        Assert.Equal("SQL", result[1].Name);
    }

    [Fact]
    public async Task CreateSkillAsync_ShouldReturnCreatedSkill()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "C#",
            Description = "Programming language"
        };

        var expectedSkill = new SkillDto
        {
            Id = Guid.NewGuid(),
            Name = "C#",
            Description = "Programming language"
        };

        var mockService = new Mock<ISkillsService>();
        mockService
            .Setup(s => s.CreateSkill(command))
            .ReturnsAsync(expectedSkill);

        var handler = new CreateSkillCommandHandler(mockService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSkill.Id, result.Id);
        Assert.Equal(expectedSkill.Name, result.Name);
        Assert.Equal(expectedSkill.Description, result.Description);

        mockService.Verify(s => s.CreateSkill(command), Times.Once);
    }
}