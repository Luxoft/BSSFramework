using System.Collections.Generic;
using System.Linq;
using Framework.Restriction;
using Framework.Validation;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public class GroupExecuteCommandRequest : ExecuteCommandRequestBase
    {
        public GroupExecuteCommandRequest()
        {
            this.TaskInstances = new List<TaskInstance>();
        }


        [Required]
        [AnyElementsValidator]
        public IList<TaskInstance> TaskInstances { get; set; }


        public IEnumerable<ExecuteCommandRequest> Split()
        {
            return from taskInstance in this.TaskInstances

                   select new ExecuteCommandRequest
                   {
                       Command = this.Command,
                       Parameters = this.Parameters,
                       TaskInstance = taskInstance
                   };
        }
    }
}