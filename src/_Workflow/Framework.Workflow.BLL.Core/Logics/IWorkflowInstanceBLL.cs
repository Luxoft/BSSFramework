using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial interface IWorkflowInstanceBLL
    {
        WorkflowInstance Start([NotNull]StartWorkflowRequest request);

        WorkflowProcessResult ExecuteCommands([NotNull]MassExecuteCommandRequest massRequest);

        WorkflowProcessResult ExecuteCommand([NotNull]ExecuteCommandRequest singleRequest);

        WorkflowProcessResult TryExecuteCommands(Guid domainObjectId, [NotNull]Command command, [NotNull]Dictionary<string, string> parameters);

        void Abort(WorkflowInstance workflowInstance);

        ITryResult<WorkflowProcessResult>[] CheckTimeouts();
    }
}
