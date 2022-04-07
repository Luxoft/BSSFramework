﻿using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Framework.Authorization.ApproveWorkflow;

public class SetPermissionStep : IStepBody
{
    [NotNull]
    private readonly IContextEvaluator<IAuthorizationBLLContext> contextEvaluator;

    public SetPermissionStep([NotNull] IScopedContextEvaluator<IAuthorizationBLLContext> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator;
    }

    public PermissionStatus Status { get; set; }

    public async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
    {
        var wfObj = (ApprovePermissionWorkflowObject)context.Workflow.Data;

        this.contextEvaluator.Evaluate(DBSessionMode.Write, ctx =>
        {
            var permission = ctx.Logics.Permission.GetById(wfObj.PermissionId, true);

            permission.Status = this.Status;

            ctx.Logics.Permission.Save(permission);
        });

        wfObj.Status = this.Status;

        return ExecutionResult.Next();
    }
}
