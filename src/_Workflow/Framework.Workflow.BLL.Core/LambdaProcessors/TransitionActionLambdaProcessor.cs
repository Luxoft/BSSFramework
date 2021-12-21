using System;
using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class TransitionActionLambdaProcessor<TBLLContext, TWorkflow> : LambdaProcessorBase<TransitionAction, Action<TBLLContext, TWorkflow>>
    {
        public TransitionActionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, transitionAction => transitionAction.Action)
        {

        }
    }
}