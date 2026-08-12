using System.Linq.Expressions;

using Anch.Core;
using Anch.IdentitySource;

namespace Framework.Database.Visitors;

public class OverrideEqualsDomainObjectMapper<TDomainObject, TIdent>(IdentityInfo<TDomainObject, TIdent> identityInfo) : IOverrideEqualsDomainObjectMapper
    where TDomainObject : notnull
    where TIdent : notnull
{
    private readonly Func<Expression, Expression> applyId =
        identityInfo.Id.Path.GetProperty().Pipe(prop => FuncHelper.Create((Expression expr) => Expression.Property(expr, prop)));

    public Maybe<BinaryExpression> TryReplace(BinaryExpression node) =>

        (from leftVal in node.Left.GetConstantValue<TDomainObject>()

         where !node.Right.GetConstantValue().HasValue

         select Expression.MakeBinary(node.NodeType, Expression.Constant(identityInfo.Id.Getter(leftVal)), this.applyId(node.Right)))

        .Or(() => from rightVal in node.Right.GetConstantValue<TDomainObject>()

                  where !node.Left.GetConstantValue().HasValue

                  select Expression.MakeBinary(node.NodeType, this.applyId(node.Left), Expression.Constant(identityInfo.Id.Getter(rightVal))));
}
