using System;
using System.Collections.Generic;

using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    ///
    /// </summary>
    public partial class Task : IMiniTask
    {
        /// <summary>
        ///
        /// </summary>
        IEnumerable<IMiniTaskMetadata> IMiniObjectMetadatasContainer<IMiniTaskMetadata>.Metadatas => this.Metadatas;

        /// <summary>
        ///
        /// </summary>
        IEnumerable<IMiniCommand> IMiniTask.Commands => this.Commands;

        /// <summary>
        ///
        /// </summary>
        string IMiniTask.StateName => this.State.Name;

        IMiniState IMiniTask.State => this.State;
    }

    [ProjectionContract(typeof(Task))]
    public interface IMiniTask : IDefaultIdentityObject, IMiniObjectMetadatasContainer<IMiniTaskMetadata>, IVisualIdentityObject, IDescriptionObject
    {
        [CustomSerialization(CustomSerializationMode.Ignore)]
        IMiniState State { get; }

        IEnumerable<IMiniCommand> Commands { get; }

        [ExpandPath("State.Name")]
        string StateName { get; }
    }
}