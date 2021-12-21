using System;
using System.Collections.Generic;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class Command : IMiniCommand
    {
        IEnumerable<IMiniCommandMetadata> IMiniObjectMetadatasContainer<IMiniCommandMetadata>.Metadatas => this.Metadatas;

        IEnumerable<IMiniCommandParameter> IMiniCommand.Parameters => this.Parameters;

        IMiniTask IMiniCommand.Task => this.Task;
    }

    [ProjectionContract(typeof(Command))]
    public interface IMiniCommand : IDefaultIdentityObject, IMiniObjectMetadatasContainer<IMiniCommandMetadata>, IVisualIdentityObject, IDescriptionObject, IOrderObject<int>
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniTask Task { get; }

        IEnumerable<IMiniCommandParameter> Parameters { get; }
    }
}