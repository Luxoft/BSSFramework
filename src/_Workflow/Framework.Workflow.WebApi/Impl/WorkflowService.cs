using System;

using Framework.Core;
using Framework.Workflow.Generated.DTO;

using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Workflow.WebApi
{
    public partial class WorkflowSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(SaveWorkflow))]
        public WorkflowIdentityDTO SaveWorkflow([FromForm] WorkflowStrictDTO workflowStrict)
        {
            if (workflowStrict == null) throw new ArgumentNullException(nameof(workflowStrict));

            return this.EvaluateC(DBSessionMode.Write, context =>
            {
                var workflowBLL = context.Logics.WorkflowFactory.Create(BLLSecurityMode.Edit);

                var domainObject = workflowBLL.GetByIdOrCreate(workflowStrict.Id);

                var mappingService = new WorkflowGraphMappingService(context, !workflowStrict.Id.IsDefault());

                mappingService.Map(workflowStrict, domainObject);

                workflowBLL.Save(domainObject);

                return domainObject.ToIdentityDTO();
            });
        }
    }
}
