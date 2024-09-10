using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.OData;

public static class StandartExpressionExtensions
{
    /// <summary>
    /// Попытка извлечь фильтр, которым можно хотябы частично профильтровать данные из бд
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <param name="filterExpression"></param>
    /// <returns></returns>
    public static Expression<Func<TDomainObject, bool>> ToRealFilter<TDomainObject>(this Expression<Func<TDomainObject, bool>> filterExpression)
    {
        if (filterExpression == null) throw new ArgumentNullException(nameof(filterExpression));

        var tree = filterExpression.ToNode();

        var virtualNodes = filterExpression.GetVirtualChains().SelectMany().ToHashSet();

        var virtualTree1 = tree.Select(expr => new
                                               {
                                                   Expr = expr,
                                                   IsVirtual = (expr as MemberExpression).Maybe(q => virtualNodes.Contains(q))
                                               });

        var virtualTree2 = virtualTree1.Select((pair, nextChildPairs) =>
                                               {
                                                   if (pair.Expr is LambdaExpression)
                                                   {
                                                       return new
                                                              {
                                                                  Expr = pair.Expr,
                                                                  IsVirtual = false
                                                              };
                                                   }
                                                   else if (pair.Expr.Type == typeof(bool) && (pair.Expr as BinaryExpression).Maybe(binExpr => binExpr.NodeType == ExpressionType.AndAlso))
                                                   {
                                                       return new
                                                              {
                                                                  Expr = pair.Expr,
                                                                  IsVirtual = false
                                                              };
                                                   }

                                                   return new
                                                          {
                                                              Expr = pair.Expr,
                                                              IsVirtual = pair.IsVirtual || nextChildPairs.Any(c => c.IsVirtual)
                                                          };
                                               });

        var dict = virtualTree2.Distinct(pair => pair.Expr).ToDictionary(pair => pair.Expr, pair => pair.IsVirtual);

        var visitor = new OverrideExpressionVisitor(e => e != null && dict[e], Expression.Constant(true));

        return filterExpression.UpdateBody(visitor);
    }

    public static bool HasVirtualProperty<T>(this ISelectOrder<T> source)
    {
        return source.Path.HasVirtualProperty();
    }

    public static bool HasVirtualProperty(this LambdaExpression expression)
    {
        return expression.GetVirtualChains().Any();
    }

    public static IEnumerable<Stack<MemberExpression>> GetVirtualChains(this LambdaExpression expression)
    {
        var visitor = new HasVirtualPropertyVisitor();

        var rootArg = expression.Parameters.Single();
        visitor.Visit(expression.Body);

        return visitor.VirtualChainCalls.Where(chain => chain.First().Expression == rootArg);
    }


    private class HasVirtualPropertyVisitor : ExpressionVisitor
    {
        public readonly List<Stack<MemberExpression>> VirtualChainCalls = new ();

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo prop && !prop.HasSystemOrPrivateField())
            {
                this.VirtualChainCalls.Add(new[] { node }.ToStack());
            }
            else
            {
                foreach (var chain in this.VirtualChainCalls.Where(chain => chain.First().Expression == node))
                {
                    chain.Push(node);
                }
            }

            return base.VisitMember(node);
        }
    }
}
