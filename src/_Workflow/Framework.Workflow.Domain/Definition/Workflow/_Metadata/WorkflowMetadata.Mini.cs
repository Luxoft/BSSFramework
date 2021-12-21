using System;

using Framework.DomainDriven.Serialization;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class WorkflowMetadata : IMiniWorkflowMetadata
    {
        IMiniWorkflow IMiniWorkflowMetadata.Workflow => this.Workflow;
    }

    [ProjectionContract(typeof(WorkflowMetadata))]
    public interface IMiniWorkflowMetadata : IMiniObjectMetadata
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniWorkflow Workflow { get; }
    }
}