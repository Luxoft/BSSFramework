using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace Framework.Database.NHibernate.Dialect;

/// <summary>
/// Represents DateTime.AddMonths method to SQL nHibernate mapping
/// </summary>
public class AddMonthsGenerator : BaseHqlGeneratorForMethod
{
    /// <summary>
    /// Creates new generator instance
    /// </summary>
    public AddMonthsGenerator() => this.SupportedMethods = [ReflectionHelper.GetMethodDefinition<DateTime?>(d => d.Value.AddMonths(0))];

    /// <summary>
    /// Creates new <see cref="HqlTreeNode"/> node that represents AddMonths method call
    /// </summary>
    public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor) =>
        treeBuilder.MethodCall("AddMonths", visitor.Visit(targetObject).AsExpression(), visitor.Visit(arguments[0]).AsExpression());
}
