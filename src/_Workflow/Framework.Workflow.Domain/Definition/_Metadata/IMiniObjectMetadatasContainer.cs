using System.Collections.Generic;

using Framework.DomainDriven.Serialization;

namespace Framework.Workflow.Domain.Definition
{
    public interface IMiniObjectMetadatasContainer<out T>
        where T : IMiniObjectMetadata
    {
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        IEnumerable<T> Metadatas { get; }
    }
}