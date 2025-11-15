
using EmployeeManagementSaaS.Application.Services;
using EmployeeManagementSaaS.Infrastructure.Reposiories;

namespace EmployeeManagementSaaS.Infrastructure.Configurations;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.AddSingleton<IEmployeesService, EmployeesService>();
        services.AddSingleton<IEmployeesRepository, EmployeesRepository>();
        services.AddSingleton<ISkillsService, SkillsService>();
        services.AddSingleton<ISkillsRepository, SkillsRepository>();
        services.AddSingleton<IAuthService, AuthService>();

        return services;
    }
}