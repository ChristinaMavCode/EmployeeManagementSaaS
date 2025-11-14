using AutoMapper;
using EmployeeManagementSaaS.Application;
using EmployeeManagementSaaS.Application.Dtos;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Products;
using EmployeeManagementSaaS.Domain.Entities;
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
    public void AutoMapper_Configuration_IsValid()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        config.AssertConfigurationIsValid();
    }
}