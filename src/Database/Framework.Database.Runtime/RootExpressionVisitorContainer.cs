using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Anch.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database;

public class RootExpressionVisitor([FromKeyedServices(RootExpressionVisitor.ElementKey)] IEnumerable<ExpressionVisitor> items)
    : ExpressionVisitor
{
    public const string RootKey = "Root";

    public const string ElementKey = "Element";

    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node) => node?.UpdateBase(items);
}
