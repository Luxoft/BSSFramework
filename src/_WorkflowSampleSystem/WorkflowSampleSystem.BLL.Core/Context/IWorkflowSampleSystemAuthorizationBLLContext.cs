using Framework.Authorization.BLL;

namespace WorkflowSampleSystem.BLL;

public interface IWorkflowSampleSystemAuthorizationBLLContext : IAuthorizationBLLContext
{
    IWorkflowApproveProcessor WorkflowApproveProcessor { get; }
}
