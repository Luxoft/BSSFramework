using System;
using System.Linq.Expressions;

using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    //public abstract class MixedLambdaProcessorBase<TDomainObject> : MixedExpressionParser<TDomainObject, WorkflowLambda>
    // where TDomainObject : PersistentDomainObjectBase
    //{
    //    protected MixedLambdaProcessorBase(INativeExpressionParser parser)
    //        : base(parser)
    //    {

    //    }
    //}

    //public abstract class LambdaProcessorBase<TDomainObject> : ExpressionParser<TDomainObject, WorkflowLambda>
    //   where TDomainObject : PersistentDomainObjectBase
    //{
    //    protected LambdaProcessorBase(INativeExpressionParser parser)
    //        : base(parser)
    //    {

    //    }
    //}

    public abstract class LambdaProcessorBase<TDomainObject, TDelegate> : DomainObjectExpressionParser<TDomainObject, WorkflowLambda, TDelegate>
        where TDomainObject : PersistentDomainObjectBase
        where TDelegate : class
    {
        protected LambdaProcessorBase(INativeExpressionParser parser, Expression<Func<TDomainObject, WorkflowLambda>> lambdaPath)
            : base(parser, lambdaPath)
        {

        }
    }


    public abstract class OverridedInputArgLambdaProcessorBase<TDomainObject, TDelegate, TInputArg> : LambdaProcessorBase<TDomainObject, TDelegate>
        where TDomainObject : PersistentDomainObjectBase
        where TDelegate : class
    {
        protected OverridedInputArgLambdaProcessorBase(INativeExpressionParser parser, Expression<Func<TDomainObject, WorkflowLambda>> lambdaPath)
            : base(parser, lambdaPath)
        {

        }


        protected sealed override Expression<TDelegate> GetInternalExpression(string lambdaValue)
        {
            var expectedArgType = this.Parser.GetArgumentType<TDelegate, TInputArg>(lambdaValue);

            if (expectedArgType == null)
            {
                return base.GetInternalExpression(lambdaValue);
            }
            else
            {
                var method = new Func<string, Expression<TDelegate>>(this.GetExpression<object>)
                            .Method
                            .GetGenericMethodDefinition()
                            .MakeGenericMethod(expectedArgType);

                return method.Invoke(this, new[] { lambdaValue }) as Expression<TDelegate>;
            }
        }

        protected abstract Expression<TDelegate> GetExpression<TExpectedArg>(string lambdaValue);
    }
}