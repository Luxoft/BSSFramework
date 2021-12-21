using System;

using Framework.DomainDriven.Serialization;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class CommandMetadata : IMiniCommandMetadata
    {
        IMiniCommand IMiniCommandMetadata.Command => this.Command;
    }

    [ProjectionContract(typeof(CommandMetadata))]
    public interface IMiniCommandMetadata : IMiniObjectMetadata
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniCommand Command { get; }
    }
}