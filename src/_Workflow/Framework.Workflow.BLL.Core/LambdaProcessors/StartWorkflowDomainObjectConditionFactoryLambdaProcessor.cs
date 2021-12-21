using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class StartWorkflowDomainObjectConditionFactoryLambdaProcessor<TBLLContext, TDomainObject, TStartupRootWorkflow> : LambdaProcessorBase<StartWorkflowDomainObjectCondition, Func<TBLLContext, TDomainObject, TDomainObject, TStartupRootWorkflow>>
    {
        public StartWorkflowDomainObjectConditionFactoryLambdaProcessor(INativeExpressionParser parser)
            : base(parser, startWorkflowDomainObjectCondition => startWorkflowDomainObjectCondition.Factory)
        {

        }


        protected override Expression<Func<TBLLContext, TDomainObject, TDomainObject, TStartupRootWorkflow>> GetInternalExpression(string lambdaValue)
        {
            var preCreatedWorkflow = ((UnaryExpression)this.Parser.Parse<Func<TBLLContext,  TDomainObject, TDomainObject, object>>(lambdaValue).Body).Operand.Type;

            var method = new Func<string, Expression<Func<TBLLContext, TDomainObject, TDomainObject, TStartupRootWorkflow>>>(this.GetExpression<TStartupRootWorkflow>)
                        .CreateGenericMethod(preCreatedWorkflow);

            return method.Invoke<Expression<Func<TBLLContext, TDomainObject, TDomainObject, TStartupRootWorkflow>>>(this, lambdaValue);
        }


        private Expression<Func<TBLLContext, TDomainObject, TDomainObject, TStartupRootWorkflow>> GetExpression<TPreCreatedWorkflow>(string lambdaValue)
        {
            var preRes = this.Parser.Parse<Func<TBLLContext, TDomainObject, TDomainObject, TPreCreatedWorkflow>>(lambdaValue);

            var convertExpr = ObjectConverter<TPreCreatedWorkflow, TStartupRootWorkflow>.Default.GetConvertExpression();

            return preRes.Select(convertExpr, true);
        }
    }
}