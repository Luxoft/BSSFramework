using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

namespace Framework.Authorization.ApproveWorkflow;

public static class WorkflowModifierExtensions
{
    public static IStepBuilder<TData, ActionStepBody> Then<TData, TStepBody>(this IWorkflowModifier<TData, TStepBody> workflowModifier, Action<IStepExecutionContext, TData> body)
        where TStepBody : IStepBody
    {
        return workflowModifier.Then(ctx => body(ctx, (TData)ctx.Workflow.Data));
    }

    public static IStepBuilder<TData, InlineStepBody> Then<TData, TStepBody>(this IWorkflowModifier<TData, TStepBody> workflowModifier, Func<IStepExecutionContext, TData, ExecutionResult> body)
        where TStepBody : IStepBody
    {
        return workflowModifier.Then(ctx => body(ctx, (TData)ctx.Workflow.Data));
    }

    //public static IContainerStepBuilder<TData, If, If> If<TData, TStepBody>(this IStepBuilder<TData, TStepBody> workflowBuilder, Expression<Func<TData, IStepExecutionContext, bool>> condition)
    //    where TStepBody : IStepBody
    //{
    //    var newStep = new WorkflowStep<If>();

    //    Expression<Func<If, bool>> inputExpr = (x => x.Condition);
    //    newStep.Inputs.Add(new MemberMapParameter(condition, inputExpr));

    //    workflowBuilder.WorkflowBuilder.AddStep(newStep);
    //    var stepBuilder = new StepBuilder<TData, If>(workflowBuilder, newStep);

    //    workflowBuilder.Step.Outcomes.Add(new ValueOutcome { NextStep = newStep.Id });

    //    return stepBuilder;
    //}
}
