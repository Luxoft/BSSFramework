using System;

using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class ParallelStateFinalEventConditionLambdaProcessor<TBLLContext, TWorkflow>
        : LambdaProcessorBase<ParallelStateFinalEvent, Func<TBLLContext, TWorkflow, IRestrictedWofkflowInstance, bool>>
    {
        public ParallelStateFinalEventConditionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, parallelStateFinalEvent => parallelStateFinalEvent.Condition)
        {

        }
    }
}