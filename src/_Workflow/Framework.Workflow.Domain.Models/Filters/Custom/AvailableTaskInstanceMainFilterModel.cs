using System;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public abstract class AvailableTaskInstanceMainFilterModelBase : DomainObjectBase
    {
        ////public bool? AssignedToMe { get; set; }

        public Guid DomainObjectId { get; set; }
    }

    public class AvailableTaskInstanceMainFilterModel : AvailableTaskInstanceMainFilterModelBase
    {
        public DomainType SourceType { get; set; }

        //public Framework.Workflow.Domain.Definition.Workflow Workflow { get; set; }
    }

    public class AvailableTaskInstanceUntypedMainFilterModel : AvailableTaskInstanceMainFilterModelBase
    {
        public string SourceTypePath { get; set; }

        //public string WorkflowPath { get; set; }
    }
}