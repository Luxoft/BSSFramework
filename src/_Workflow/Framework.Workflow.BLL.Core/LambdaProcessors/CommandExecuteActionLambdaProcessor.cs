using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.ExpressionParsers;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class CommandExecuteActionLambdaProcessor<TBLLContext, TWorkflow, TCommand> :
        OverridedInputArgLambdaProcessorBase<Command, Action<TBLLContext, TWorkflow, TCommand>, TCommand>
    {
        public CommandExecuteActionLambdaProcessor(INativeExpressionParser parser)
            : base(parser, command => command.ExecuteAction)
        {

        }

        protected override Expression<Action<TBLLContext, TWorkflow, TCommand>> GetExpression<TExpectedCommand>(string lambdaValue)
        {
            var expression = this.Parser.Parse<Action<TBLLContext, TWorkflow, TExpectedCommand>>(lambdaValue);

            var convertExpr = ObjectConverter<TCommand, TExpectedCommand>.Default.GetConvertExpression();

            return expression.OverrideThird(convertExpr);
        }
    }
}