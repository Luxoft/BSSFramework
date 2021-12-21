using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Core;
using Framework.ExpressionParsers;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class ParallelStateStartItemFactoryLambdaProcessor<TBLLContext, TWorkflow, TStartupSubWorkflow> : LambdaProcessorBase<ParallelStateStartItem, Func<TBLLContext, TWorkflow, IEnumerable<TStartupSubWorkflow>>>
    {
        public ParallelStateStartItemFactoryLambdaProcessor(INativeExpressionParser parser)
            : base(parser, parallelStateStartItem => parallelStateStartItem.Factory)
        {

        }


        protected override Expression<Func<TBLLContext, TWorkflow, IEnumerable<TStartupSubWorkflow>>> GetInternalExpression(string lambdaValue)
        {
            var preCreatedSubWorkflow = ((UnaryExpression)this.Parser.Parse<Func<TBLLContext, TWorkflow, object>>(lambdaValue).Body).Operand.Type.GetCollectionElementType();

            var method = new Func<string, Expression<Func<TBLLContext, TWorkflow, IEnumerable<TStartupSubWorkflow>>>>(this.GetExpression<TStartupSubWorkflow>)
                        .CreateGenericMethod(preCreatedSubWorkflow);


            return method.Invoke<Expression<Func<TBLLContext, TWorkflow, IEnumerable<TStartupSubWorkflow>>>>(this, lambdaValue);
        }

        private Expression<Func<TBLLContext, TWorkflow, IEnumerable<TStartupSubWorkflow>>> GetExpression<TPreCreatedSubWorkflow>(string lambdaValue)
        {
            var preRes = this.Parser.Parse<Func<TBLLContext, TWorkflow, IEnumerable<TPreCreatedSubWorkflow>>>(lambdaValue);

            var convertExpr = new WorkflowStartupObjectConverter<TPreCreatedSubWorkflow, TStartupSubWorkflow>()
                             .GetConvertExpression()
                             .ToEnumerable();

            return preRes.Select(convertExpr, true);
        }

        private class WorkflowStartupObjectConverter<TPreCreatedSubWorkflow, TCreatedSubWorkflow> : ExpressionConverter<TPreCreatedSubWorkflow, TCreatedSubWorkflow>
        {
            protected override IEnumerable<System.Reflection.MemberInfo> GetMembers()
            {
                return base.GetMembers().Where(member => member.Name != WorkflowParameter.OwnerWorkflowName);
            }

            protected override ExpressionConverter<TSubSource, TSubTarget> GetSubConverter<TSubSource, TSubTarget>()
            {
                return new WorkflowStartupObjectConverter<TSubSource, TSubTarget>();
            }
        }
    }
}