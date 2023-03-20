using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public class OverrideMemberInfoVisitor : ExpressionVisitor
{
    private readonly MemberInfo _member;
    private readonly Func<MemberExpression, Expression> _mapExpression;

    public OverrideMemberInfoVisitor(MemberInfo member, Func<MemberExpression, Expression> mapExpression)
    {
        if (member == null) throw new ArgumentNullException(nameof(member));
        if (mapExpression == null) throw new ArgumentNullException(nameof(mapExpression));


        this._member = member;
        this._mapExpression = mapExpression;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Member == this._member)
        {
            var visitedNode = Expression.MakeMemberAccess(this.Visit(node.Expression), this._member);

            return this._mapExpression(visitedNode);
        }

        return base.VisitMember(node);
    }
}
