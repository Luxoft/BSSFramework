using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace AttachmentsSampleSystem.IntegrationTests.NH
{
    public class PropertyFullTextContainsGenerator : BaseHqlGeneratorForMethod
    {
        public PropertyFullTextContainsGenerator()
        {
            this.SupportedMethods = new[] { DialectExtensions.GetPropetyFullTextContainsMethodInfo() };
        }

        public override HqlTreeNode BuildHql(
            MethodInfo method,
            Expression targetObject,
            ReadOnlyCollection<Expression> arguments,
            HqlTreeBuilder treeBuilder,
            IHqlExpressionVisitor visitor)
        {
            var args = new[]
            {
                visitor.Visit(arguments[0]).AsExpression(),
                visitor.Visit(arguments[1]).AsExpression()
            };

            return treeBuilder.BooleanMethodCall("contains", args);
        }
    }
}
