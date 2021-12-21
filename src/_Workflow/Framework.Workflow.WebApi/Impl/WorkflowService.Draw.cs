using System;
using System.IO;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;
using Framework.Workflow.Generated.DTO;
using Framework.Workflow.Graphviz;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Workflow.WebApi
{
    public partial class WorkflowSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(DrawWorkflow))]
        public Stream DrawWorkflow([FromForm] DrawWorkflowRequest request)
        {
            return this.DrawWorkflowInternal(request).ToMemoryStream();
        }

        private byte[] DrawWorkflowInternal([FromForm] DrawWorkflowRequest request)
        {
            return this.EvaluateC(DBSessionMode.Read, context =>
            {
                var workflow = context.Logics.WorkflowFactory.Create(BLLSecurityMode.View).GetById(request.Workflow.Id);

                var graph = workflow.GetDotGraph(request.Dpi);

                return this.ServiceEnvironment.DotVisualizer.Render(graph, request.Format);
            });
        }
    }
}
