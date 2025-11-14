
using EmployeeManagementSaaS.Infrastructure.Reposiories;

namespace EmployeeManagementSaaS.Infrastructure.Configurations;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.AddSingleton<ISkillsService, SkillsService>();
        services.AddSingleton<ISkillsRepository, SkillsRepository>();

        return services;
    }
}