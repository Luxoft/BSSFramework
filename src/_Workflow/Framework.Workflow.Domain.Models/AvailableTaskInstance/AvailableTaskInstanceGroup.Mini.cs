using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Projection.Contract;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceGroup : IMiniAvailableTaskInstanceGroup
    {
        IMiniTask IMiniAvailableTaskInstanceGroup.Task => this.Task;

        IEnumerable<IMiniAvailableTaskInstanceGroupItem> IMiniAvailableTaskInstanceGroup.Items => this.Items;
    }


    [ProjectionContract(typeof(AvailableTaskInstanceGroup))]
    public interface IMiniAvailableTaskInstanceGroup
    {
        [DetailRole(true)]
        IMiniTask Task { get; }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        IEnumerable<IMiniAvailableTaskInstanceGroupItem> Items { get; }
    }
}