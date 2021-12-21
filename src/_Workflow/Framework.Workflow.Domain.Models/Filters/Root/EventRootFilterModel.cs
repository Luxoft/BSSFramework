using System;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public class EventRootFilterModel : DomainObjectContextFilterModel<Event>
    {
        public Workflow.Domain.Definition.Workflow Workflow { get; set; }
    }
}