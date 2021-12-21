using System;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class StateDomainObjectConditionLambdaProcessor<TBLLContext, TWorkflow> : LambdaProcessorBase<StateDomainObjectEvent, Func<TBLLContext, TWorkflow, bool>>
    {
        public StateDomainObjectConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, stateTimeoutEvent => stateTimeoutEvent.Condition)
        {

        }
    }
}