using Framework.Restriction;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.Domain
{
    public class AvailableCommandFilterModel : DomainObjectContextFilterModel<Command>
    {
        [Required]
        public TaskInstance TaskInstance { get; set; }
    }
}
