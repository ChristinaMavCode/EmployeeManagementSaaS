using AutoMapper;
using EmployeeManagementSaaS.Application;
using EmployeeManagementSaaS.Application.Dtos;
using EmployeeManagementSaaS.Application.Interfaces;
using EmployeeManagementSaaS.Application.Products;
using EmployeeManagementSaaS.Application.Services;
using EmployeeManagementSaaS.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagementSaaS.UnitTests;

public class AutoMapperTests
{
    public AutoMapperTests()
    {
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