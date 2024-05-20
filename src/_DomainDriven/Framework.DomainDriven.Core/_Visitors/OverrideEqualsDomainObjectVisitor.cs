using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven._Visitors;

public class OverrideEqualsDomainObjectVisitor<TIdent> : ExpressionVisitor
{
    private static readonly ConcurrentDictionary<PropertyInfo, OverrideEqualsDomainObjectVisitor<TIdent>> Cache = new ConcurrentDictionary<PropertyInfo, OverrideEqualsDomainObjectVisitor<TIdent>>();

    private readonly PropertyInfo idProperty;

    private OverrideEqualsDomainObjectVisitor(PropertyInfo idProperty)
    {
        if (idProperty == null) throw new ArgumentNullException(nameof(idProperty));

        this.idProperty = idProperty;
    }

    /// <summary> Returns <see cref="OverrideEqualsDomainObjectVisitor{TIdent}"/> for specified <paramref name="property"/>
    /// </summary>
    /// <param name="property">Property to get ExpressionVisitor for</param>
    /// <returns>Expression Visitor</returns>
    public static OverrideEqualsDomainObjectVisitor<TIdent> GetOrCreate(PropertyInfo property)
    {
        return Cache.GetOrAdd(property, pInfo => new OverrideEqualsDomainObjectVisitor<TIdent>(pInfo));
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var applyId = FuncHelper.Create((Expression expr) => Expression.Property(expr, this.idProperty));
        var idPropertyDeclaringType = this.idProperty.DeclaringType;

        // TODO gtsaplin: it's too complicated code, refactor
        var request = from _ in Maybe.Return()

                      where node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual

                      where idPropertyDeclaringType != null && (idPropertyDeclaringType.IsAssignableFrom(node.Left.Type) || idPropertyDeclaringType.IsAssignableFrom(node.Right.Type))

                      let left = this.Visit(node.Left)

                      let right = this.Visit(node.Right)

                      from res in (from leftVal in left.GetMemberConstValue<IIdentityObject<TIdent>>()

                                   where !right.GetMemberConstValue().HasValue

                                   select Expression.MakeBinary(node.NodeType, Expression.Constant(leftVal.Id), applyId(right)))

                              .Or(() => from rightVal in right.GetMemberConstValue<IIdentityObject<TIdent>>()

                                        where !left.GetMemberConstValue().HasValue

                                        select Expression.MakeBinary(node.NodeType, applyId(left), Expression.Constant(rightVal.Id)))

                      select res;

        return request.GetValueOrDefault(() => base.VisitBinary(node));
    }
}
