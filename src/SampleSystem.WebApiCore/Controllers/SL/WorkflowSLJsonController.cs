using System;

using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Exceptions;

namespace SampleSystem.WebApiCore.Controllers
{
    public class WorkflowSLJsonController : Framework.Workflow.WebApi.WorkflowSLJsonController
    {
        public WorkflowSLJsonController(IWorkflowServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }
    }
}
