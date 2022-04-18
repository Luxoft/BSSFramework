using System;

using Microsoft.Extensions.DependencyInjection;

using WorkflowCore.Interface;

namespace Framework.Authorization.ApproveWorkflow;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddAuthWorkflow(this IServiceCollection services)
    {
        return services
               .AddScoped<IWorkflowApproveProcessor, WorkflowApproveProcessor>()

               .AddTransient<StartWorkflow>()
               .AddTransient<PublishEvent>()
               .AddTransient<SendFinalEvent>()
               .AddTransient<TerminateWorkflowStep>()

               .AddTransient<CalcHasAccessStep>()
               .AddTransient<CanAutoApproveStep>()
               .AddTransient<LogAccessErrorStep>()
               .AddTransient<SetPermissionStatusStep>();
    }

    public static void RegisterAuthWorkflow(this IServiceProvider serviceProvider)
    {
        var host = serviceProvider.GetRequiredService<IWorkflowHost>();

        host.RegisterWorkflow<__ApproveOperation_Workflow, ApproveOperationWorkflowObject>();
        host.RegisterWorkflow<__ApprovePermission_Workflow, ApprovePermissionWorkflowObject>();
    }
}
