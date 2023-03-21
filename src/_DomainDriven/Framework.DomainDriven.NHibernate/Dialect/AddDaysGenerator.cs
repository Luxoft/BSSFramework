using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents DateTime.AddDays method to SQL nHibernate mapping
/// </summary>
public class AddDaysGenerator : BaseHqlGeneratorForMethod
{
    /// <summary>
    /// Creates new generator instance
    /// </summary>
    public AddDaysGenerator()
    {
        this.SupportedMethods = new[] { ReflectionHelper.GetMethodDefinition<DateTime?>(d => d.Value.AddDays(0)) };
    }

    /// <summary>
    /// Creates new <see cref="HqlTreeNode"/> node that represents AddDays method call
    /// </summary>
    public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor)
    {
        return treeBuilder.MethodCall("AddDays", visitor.Visit(targetObject).AsExpression(), visitor.Visit(arguments[0]).AsExpression());
    }
}
