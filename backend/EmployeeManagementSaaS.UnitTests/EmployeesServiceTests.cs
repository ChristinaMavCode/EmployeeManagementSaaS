using AutoMapper;
using EmployeeManagementSaaS.Application;
using EmployeeManagementSaaS.Application.Commands;
using EmployeeManagementSaaS.Application.Dtos;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Products;
using EmployeeManagementSaaS.Application.Services;
using EmployeeManagementSaaS.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Xml.Linq;

namespace EmployeeManagementSaaS.UnitTests;

public class EmployeesServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IEmployeesRepository> _mockEmployeesRepo;

    public EmployeesServiceTests()
    {
        // AutoMapper configuration for tests
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Employee, EmployeeDto>();
            cfg.CreateMap<Skill, SkillDto>();
        });
        _mapper = config.CreateMapper();

        _mockEmployeesRepo = new Mock<IEmployeesRepository>();
    }

    [Fact]
    public async Task GetSkillsAsync_ReturnsMappedDtos()
    {
        // Arrange
        var data = new List<Employee>
        {
            new Employee { Id = new Guid(), Name = "Christina", Surname = "Mavridi", CreationAt = DateTime.Now },
            new Employee { Id = new Guid(), Name = "Kostas", Surname = "Mavridis", CreationAt = DateTime.Now }
        };
        _mockEmployeesRepo.Setup(r => r.GetEmployees()).ReturnsAsync(data);

        var service = new EmployeesService(Mock.Of<ILogger<EmployeesService>>(), _mockEmployeesRepo.Object, _mapper);

        // Act
        var result = await service.GetEmployees();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Christina Mavridi", result[0].FullName);
        Assert.Equal("Kostas Mavridis", result[1].FullName);
    }

    [Fact]
    public async Task AssignSkillAsync_ShouldReturnEmployeeWithSkill()
    {
        // Arrange
        var command = new AssignSkillToEmployeeCommand
        {
            EmployeeID = "8bb6066d-07c1-4d30-baa8-950d23e3bd2e",
            SkillID = "b8763613-919e-4c70-ae05-9d6562e02541"
        };

        var expectedEmployee = new EmployeeDto
        {
            Id =  Guid.Parse("8bb6066d-07c1-4d30-baa8-950d23e3bd2e"),
            Name = "Christina",
            Surname = "Mavridi",
            Skills = "C#"
        };

        var mockService = new Mock<IEmployeesService>();
        mockService
            .Setup(s => s.AssignSkillToEmployee(command))
            .ReturnsAsync(expectedEmployee);

        var handler = new AssignSkillToEmployeeCommandHandler(mockService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEmployee.Id, result.Id);
        Assert.Equal(expectedEmployee.Name, result.Name);
        Assert.Equal(expectedEmployee.Surname, result.Surname);
        Assert.Equal(expectedEmployee.Skills, result.Skills);

        mockService.Verify(s => s.AssignSkillToEmployee(command), Times.Once);
    }
}