using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;
using Framework.Projection.Contract;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceGroupItem : IMiniAvailableTaskInstanceGroupItem
    {
        IEnumerable<IMiniTaskInstance> IMiniAvailableTaskInstanceGroupItem.TaskInstances
        {
            get { return this.TaskInstances; }
        }

        public Guid[] CommandsIdents
        {
            get { return this.Commands.ToArray(c => c.Id); }
        }
    }


    [ProjectionContract(typeof(AvailableTaskInstanceGroupItem))]
    public interface IMiniAvailableTaskInstanceGroupItem
    {
        string[] Path { get; }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        IEnumerable<IMiniTaskInstance> TaskInstances { get; }

        Guid[] CommandsIdents { get; }
    }
}