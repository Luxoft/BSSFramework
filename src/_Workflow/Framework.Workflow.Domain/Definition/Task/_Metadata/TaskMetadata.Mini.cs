using System;

using Framework.DomainDriven.Serialization;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class TaskMetadata : IMiniTaskMetadata
    {
        IMiniTask IMiniTaskMetadata.Task => this.Task;
    }

    [ProjectionContract(typeof(TaskMetadata))]
    public interface IMiniTaskMetadata : IMiniObjectMetadata
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniTask Task { get; }
    }
}