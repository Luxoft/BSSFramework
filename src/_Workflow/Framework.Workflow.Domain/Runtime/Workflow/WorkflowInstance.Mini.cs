using System;

using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Runtime
{
    public partial class WorkflowInstance : IMiniWorkflowInstance
    {
    }

    [ProjectionContract(typeof(WorkflowInstance))]
    public interface IMiniWorkflowInstance : IDefaultIdentityObject, IVisualIdentityObject
    {
    }
}