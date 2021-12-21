using System;
using Framework.ExpressionParsers;

namespace Framework.Workflow.BLL
{
    public class WorkflowActiveConditionLambdaProcessor<TBLLContext, TWorkflow> : LambdaProcessorBase<Framework.Workflow.Domain.Definition.Workflow, Func<TBLLContext, TWorkflow, bool>>
    {
        public WorkflowActiveConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, workflow => workflow.ActiveCondition)
        {

        }
    }
}