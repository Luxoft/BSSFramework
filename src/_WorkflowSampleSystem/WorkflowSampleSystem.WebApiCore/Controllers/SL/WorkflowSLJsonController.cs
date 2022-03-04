using Framework.Exceptions;

using Framework.Workflow.Environment;

namespace WorkflowSampleSystem.WebApiCore.Controllers
{
    public class WorkflowSLJsonController : Framework.Workflow.WebApi.WorkflowSLJsonController
    {
        public WorkflowSLJsonController(IWorkflowServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
                : base(environment, exceptionProcessor)
        {
        }
    }
}
