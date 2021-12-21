using System;
using System.ComponentModel;

using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public class TaskInstanceRootFilterModel : DomainObjectContextFilterModel<TaskInstance>
    {
        public TaskInstanceRootFilterModel()
        {
            this.IsAvailable = true;
        }


        [DefaultValue(true)]
        public bool? IsAvailable { get; set; }

        // not work
        public bool? AssignedToMe { get; set; }

        //[Required]
        public Guid DomainObjectId { get; set; }

        public WorkflowInstance WorkflowInstance { get; set; }

        public Framework.Workflow.Domain.Definition.Workflow WorkflowDefinition { get; set; }
    }
}