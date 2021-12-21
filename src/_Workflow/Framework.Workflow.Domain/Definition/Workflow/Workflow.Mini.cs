using System;
using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    public partial class Workflow : IMiniWorkflow, IVisualWorkflow
    {
        IEnumerable<IMiniWorkflowMetadata> IMiniObjectMetadatasContainer<IMiniWorkflowMetadata>.Metadatas => this.Metadatas;

        Guid? IMiniWorkflow.OwnerId => this.Owner.TryGetId();

        IVisualWorkflow IMiniWorkflow.Owner => this.Owner;

        IVisualDomainType IMiniWorkflow.DomainType => this.DomainType;
    }

    [ProjectionContract(typeof(Workflow))]
    public interface IMiniWorkflow : IDefaultIdentityObject, IMiniObjectMetadatasContainer<IMiniWorkflowMetadata>, IVisualIdentityObject, IDescriptionObject
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IVisualWorkflow Owner { get; }

        [ExpandPath("Owner.Id")]
        Guid? OwnerId { get; }

        IVisualDomainType DomainType { get; }
    }

    [ProjectionContract(typeof(Workflow))]
    public interface IVisualWorkflow : IDefaultIdentityObject, IVisualIdentityObject
    {
    }
}
