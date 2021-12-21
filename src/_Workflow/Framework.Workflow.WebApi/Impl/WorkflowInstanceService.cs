using System;

using Framework.Workflow.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Workflow.WebApi
{
    public partial class WorkflowSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(StartWorkflow))]
        public WorkflowInstanceIdentityDTO StartWorkflow([FromForm] StartWorkflowRequestStrictDTO startWorkflowRequest)
        {
            return this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var workflowInstanceBLL = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(BLLSecurityMode.Edit);

                var startWorkflowRequestDomain = startWorkflowRequest.ToDomainObject(evaluateData.MappingService);

                return workflowInstanceBLL.Start(startWorkflowRequestDomain).ToIdentityDTO();
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(AbortWorkflow))]
        public void AbortWorkflow([FromForm] WorkflowInstanceIdentityDTO workflowInstanceIdentityDTO)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                var workflowInstanceBLL = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(BLLSecurityMode.Edit);

                var workflowInstance = workflowInstanceIdentityDTO.ToDomainObject(evaluateData.MappingService);

                workflowInstanceBLL.Abort(workflowInstance);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ExecuteCommand))]
        public void ExecuteCommand([FromForm] ExecuteCommandRequestStrictDTO executeCommandRequest)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                if (executeCommandRequest == null) throw new ArgumentNullException(nameof(executeCommandRequest));

                var workflowInstanceBLL = evaluateData.Context.Logics.WorkflowInstance;

                workflowInstanceBLL.ExecuteCommand(executeCommandRequest.ToDomainObject(evaluateData.MappingService));
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ExecuteCommands))]
        public void ExecuteCommands([FromForm] MassExecuteCommandRequestStrictDTO executeCommandRequest)
        {
            this.Evaluate(DBSessionMode.Write, evaluateData =>
            {
                if (executeCommandRequest == null) throw new ArgumentNullException(nameof(executeCommandRequest));

                var workflowInstanceBLL = evaluateData.Context.Logics.WorkflowInstance;

                workflowInstanceBLL.ExecuteCommands(executeCommandRequest.ToDomainObject(evaluateData.MappingService));
            });
        }
    }
}
