using System.Collections.Generic;

using Framework.Persistent;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceWorkflowGroup : DomainObjectBase
    {
        public AvailableTaskInstanceWorkflowGroup()
        {
            this.Items = new List<AvailableTaskInstanceGroup>();
        }


        [DetailRole(true)]
        public Definition.Workflow Workflow { get; set; }

        public Definition.WorkflowSource Source { get; set; }

        public IList<AvailableTaskInstanceGroup> Items { get; set; }
    }
}