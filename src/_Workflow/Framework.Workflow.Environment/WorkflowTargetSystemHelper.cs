using Framework.Persistent;

namespace Framework.Workflow.Environment;

public static class WorkflowTargetSystemHelper
{
    public static readonly string WorkflowName = typeof(Framework.Workflow.Domain.PersistentDomainObjectBase).GetTargetSystemName();
}
