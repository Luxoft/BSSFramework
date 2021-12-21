using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Serialization;

using Framework.Restriction;
using Framework.Validation;

namespace Framework.Workflow.Domain
{
    public class MassExecuteCommandRequest : DomainObjectBase
    {
        public MassExecuteCommandRequest()
        {
            this.Groups = new List<GroupExecuteCommandRequest>();
        }


        [Required]
        [AnyElementsValidator]
        public IList<GroupExecuteCommandRequest> Groups { get; set; }

        [Required]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public bool UniqueTaskInstances
        {
            get { return !this.Groups.SelectMany(g => g.TaskInstances).HasDuplicates(); }
        }


        public IEnumerable<ExecuteCommandRequest> Split()
        {
            return from @group in this.Groups

                   from request in @group.Split()

                   select request;
        }
    }
}