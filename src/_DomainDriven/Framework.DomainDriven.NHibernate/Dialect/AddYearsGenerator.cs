using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents DateTime.AddYears method to SQL nHibernate mapping
/// </summary>
public class AddYearsGenerator : BaseHqlGeneratorForMethod
{
    /// <summary>
    /// Creates new generator instance
    /// </summary>
    public AddYearsGenerator() =>
            this.SupportedMethods = new[] { ReflectHelper.GetMethodDefinition<DateTime?>(d => d.Value.AddYears(0)) };

    /// <summary>
    /// Creates new <see cref="HqlTreeNode"/> node that represents AddYears method call
    /// </summary>
    public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor) =>
            treeBuilder.MethodCall(
                                   "AddYears",
                                   visitor.Visit(targetObject).AsExpression(),
                                   visitor.Visit(arguments[0]).AsExpression());
}
