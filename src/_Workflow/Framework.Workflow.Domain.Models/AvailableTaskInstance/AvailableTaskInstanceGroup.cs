using System.Collections.Generic;

using Framework.Persistent;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceGroup : DomainObjectBase
    {
        public AvailableTaskInstanceGroup()
        {
            this.Items = new List<AvailableTaskInstanceGroupItem>();
        }


        [DetailRole(true)]
        public Task Task { get; set; }

        public IList<AvailableTaskInstanceGroupItem> Items { get; set; }
    }
}