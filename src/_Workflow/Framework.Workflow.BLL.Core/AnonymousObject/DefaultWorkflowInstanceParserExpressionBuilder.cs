using System;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    internal class DefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>
        : DefaultParameterizedObjectParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase, WorkflowInstance>

        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        public DefaultWorkflowInstanceParserExpressionBuilder(Type anonType)
            : base(anonType)
        {

        }


        protected virtual DefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase> GetOtherBuilder(Type otherAnonType)
        {
            return new DefaultWorkflowInstanceParserExpressionBuilder<TBLLContext, TPersistentDomainObjectBase>(otherAnonType);
        }

        protected override Expression GetMemberBinding(Expression contextExpr, Expression wfInstanceExpr, PropertyInfo property, Expression parametersExpr)
        {
            if (property.Name == WorkflowParameter.OwnerWorkflowName)
            {
                return ExpressionHelper.Create((WorkflowInstance wf) => wf.Owner)
                                       .GetBodyWithOverrideParameters(wfInstanceExpr)
                                       .WithPipe(ownerWfInstanceExpr => this.GetOtherBuilder(property.PropertyType).GetParseExpression(contextExpr, ownerWfInstanceExpr));
            }
            else
            {
                return base.GetMemberBinding(contextExpr, wfInstanceExpr, property, parametersExpr);
            }
        }
    }
}