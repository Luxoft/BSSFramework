using System;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class ConditionStateConditionLambdaProcessor<TBLLContext, TWorkflow> : LambdaProcessorBase<ConditionState, Func<TBLLContext, TWorkflow, bool>>
    {
        public ConditionStateConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, conditionState => conditionState.Condition)
        {

        }
    }
}