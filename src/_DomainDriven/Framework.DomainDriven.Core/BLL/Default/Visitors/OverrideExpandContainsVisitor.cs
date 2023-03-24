using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.HierarchicalExpand;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL;

internal class OverrideExpandContainsVisitor<TBLLContext, TIdent> : ExpressionVisitor

        where TBLLContext : class, IHierarchicalObjectExpanderFactoryContainer<TIdent>
{
    private readonly TBLLContext context;

    private readonly PropertyInfo idProperty;


    public OverrideExpandContainsVisitor([NotNull] TBLLContext context, PropertyInfo idProperty)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.idProperty = idProperty;
    }


    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.IsGenericMethodImplementation(Framework.QueryLanguage.MethodInfoHelper.ExpandContainsMethod))
        {
            var source = node.Arguments[0];

            var ident = node.Arguments[1].GetDeepMemberConstValue<TIdent>().GetValue();

            var expandType = node.Arguments[2].GetDeepMemberConstValue<HierarchicalExpandType>().GetValue();

            var expandedIdents = this.context.HierarchicalObjectExpanderFactory.Create(source.Type).Expand(new[] { ident }, expandType);

            var enumerableContainsMethod = new Func<TIdent, bool>(expandedIdents.Contains).Method;

            var idSourceExpr = Expression.Property(source, this.idProperty);

            var containsExpr = Expression.Call(enumerableContainsMethod, Expression.Constant(expandedIdents), idSourceExpr);

            return containsExpr;
        }
        else
        {
            return base.VisitMethodCall(node);
        }
    }
}
