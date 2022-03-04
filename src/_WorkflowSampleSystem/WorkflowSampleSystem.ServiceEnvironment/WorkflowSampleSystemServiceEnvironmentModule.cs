using Framework.Graphviz;
using Framework.Graphviz.Dot;
using Framework.Workflow.Environment;

using JetBrains.Annotations;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.ServiceEnvironment;

public class WorkflowSampleSystemServiceEnvironmentModule : WorkflowServiceEnvironmentModule<WorkflowSampleSystemServiceEnvironment, WorkflowSampleSystemBLLContextContainer, IWorkflowSampleSystemBLLContext, PersistentDomainObjectBase>
{
    public WorkflowSampleSystemServiceEnvironmentModule([NotNull] WorkflowSampleSystemServiceEnvironment mainServiceEnvironment, [NotNull] IDotVisualizer<DotGraph> dotVisualizer) : base(mainServiceEnvironment, dotVisualizer)
    {
    }
}
