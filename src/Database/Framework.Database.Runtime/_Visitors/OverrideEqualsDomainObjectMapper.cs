using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.IdentitySource;
using CommonFramework.Maybe;

namespace Framework.Database._Visitors;

public class OverrideEqualsDomainObjectMapper<TDomainObject, TIdent>(IdentityInfo<TDomainObject, TIdent> identityInfo) : IOverrideEqualsDomainObjectMapper
    where TDomainObject : notnull
    where TIdent : notnull
{
    private readonly Func<Expression, Expression> applyId =
        identityInfo.Id.Path.GetProperty().Pipe(prop => FuncHelper.Create((Expression expr) => Expression.Property(expr, prop)));

    public Maybe<BinaryExpression> TryReplace(BinaryExpression node) =>

        (from leftVal in node.Left.GetMemberConstValue<TDomainObject>()

         where !node.Right.GetMemberConstValue().HasValue

         select Expression.MakeBinary(node.NodeType, Expression.Constant(identityInfo.Id.Getter(leftVal)), this.applyId(node.Right)))

        .Or(() => from rightVal in node.Right.GetMemberConstValue<TDomainObject>()

                  select Expression.MakeBinary(node.NodeType, this.applyId(node.Left), Expression.Constant(identityInfo.Id.Getter(rightVal))));
}
