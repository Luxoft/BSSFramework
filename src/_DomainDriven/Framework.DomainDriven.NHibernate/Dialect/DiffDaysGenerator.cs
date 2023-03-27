using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;
using NHibernate.Util;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Represents DateTime subtract method to SQL nHibernate mapping
/// </summary>
public class DiffDaysHqlGenerator : BaseHqlGeneratorForMethod
{
    public DiffDaysHqlGenerator() =>
            this.SupportedMethods = new[] { ReflectHelper.GetMethodDefinition((DateTime x) => x.DiffDays(DateTime.MinValue)) };

    public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor) =>
            treeBuilder.MethodCall(
                                   "DiffDays",
                                   visitor.Visit(arguments[0]).AsExpression(),
                                   visitor.Visit(arguments[1]).AsExpression());
}
