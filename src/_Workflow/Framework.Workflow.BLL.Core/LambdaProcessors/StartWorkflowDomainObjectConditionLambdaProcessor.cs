using System;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class StartWorkflowDomainObjectConditionLambdaProcessor<TDomainObject> : LambdaProcessorBase<StartWorkflowDomainObjectCondition, Func<TDomainObject, TDomainObject, bool>>
    {
        public StartWorkflowDomainObjectConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, startWorkflowDomainObjectCondition => startWorkflowDomainObjectCondition.Condition)
        {

        }
    }
}