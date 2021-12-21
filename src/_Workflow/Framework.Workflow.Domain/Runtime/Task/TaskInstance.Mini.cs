using System;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Runtime
{
    public partial class TaskInstance : IMiniTaskInstance
    {
        string IMiniTaskInstance.WorkflowInstanceName => this.Workflow.Name;

        IMiniWorkflowInstance IMiniTaskInstance.Workflow => this.Workflow;
    }

    [ProjectionContract(typeof(TaskInstance))]
    public interface IMiniTaskInstance : IDefaultIdentityObject
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniWorkflowInstance Workflow { get; }

        [ExpandPath("Workflow.Name")]
        string WorkflowInstanceName { get; }
    }
}