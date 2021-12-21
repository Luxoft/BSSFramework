using System;
using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class StateTimeoutConditionLambdaProcessor<TBLLContext, TWorkflow> : LambdaProcessorBase<StateTimeoutEvent, Func<TBLLContext, TWorkflow, DateTime>>
    {
        public StateTimeoutConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, stateTimeoutEvent => stateTimeoutEvent.Condition)
        {

        }
    }
}