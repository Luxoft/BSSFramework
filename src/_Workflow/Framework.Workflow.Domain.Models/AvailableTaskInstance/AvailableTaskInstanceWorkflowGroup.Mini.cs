using System.Collections.Generic;

using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

using Framework.Projection;
using Framework.Projection.Contract;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceWorkflowGroup : IMiniAvailableTaskInstanceWorkflowGroup
    {
        IMiniWorkflow IMiniAvailableTaskInstanceWorkflowGroup.Workflow
        {
            get { return this.Workflow; }
        }

        IEnumerable<IMiniAvailableTaskInstanceGroup> IMiniAvailableTaskInstanceWorkflowGroup.Items
        {
            get { return this.Items; }
        }

        IMiniWorkflowSource IMiniAvailableTaskInstanceWorkflowGroup.Source
        {
            get { return this.Source; }
        }
    }


    [ProjectionContract(typeof(AvailableTaskInstanceWorkflowGroup))]
    public interface IMiniAvailableTaskInstanceWorkflowGroup
    {
        [DetailRole(true)]
        IMiniWorkflow Workflow { get; }

        [DetailRole(true)]
        IMiniWorkflowSource Source { get; }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        IEnumerable<IMiniAvailableTaskInstanceGroup> Items { get; }
    }
}