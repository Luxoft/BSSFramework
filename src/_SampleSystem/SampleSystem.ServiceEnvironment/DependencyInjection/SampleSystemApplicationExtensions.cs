using System;

using Framework.Cap;
using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemApplicationExtensions
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterWorkflowCore(configuration)
                       .RegisterApplicationServices()
                       .AddCapBss(configuration.GetConnectionString("DefaultConnection"))
                       .AddLogging();
    }

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExampleServiceForRepository, ExampleServiceForRepository>();

        return services;
    }

    private static IServiceCollection RegisterWorkflowCore(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorkflowCoreConnection");

        services.AddSingleton<WorkflowManager>();
        services.AddSingletonFrom<IWorkflowManager, WorkflowManager>();

        services.AddWorkflow(x => x.UseSqlServer(connectionString, true, true));

        return services;
    }
}
