using System.Collections.Generic;

using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public partial class AvailableTaskInstanceGroupItem : DomainObjectBase
    {
        public AvailableTaskInstanceGroupItem()
        {
            this.TaskInstances = new List<TaskInstance>();

            this.Commands = new List<Command>();

            this.Path = new string[0];
        }


        public string[] Path { get; set; }

        public IList<TaskInstance> TaskInstances { get; set; }

        public IList<Command> Commands { get; set; }
    }
}