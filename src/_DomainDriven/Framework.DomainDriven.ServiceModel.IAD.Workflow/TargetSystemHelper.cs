using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class WorkflowTargetSystemHelper
    {
        public static readonly string WorkflowName = typeof(Framework.Workflow.Domain.PersistentDomainObjectBase).GetTargetSystemName();
    }
}
