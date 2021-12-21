using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core
{
    public class OverridePropertyInfoVisitor<TSource, TProperty> : OverrideMemberInfoVisitor
    {
        public OverridePropertyInfoVisitor(Expression<Func<TSource, TProperty>> memberExpr, Expression<Func<TProperty, TProperty>> mapExpr)
            : base(typeof(TSource).GetProperty(memberExpr.GetMemberName(), true),
                   sourceExpr => mapExpr.Body.Override(mapExpr.Parameters.Single(), sourceExpr))
        {

        }
    }
}