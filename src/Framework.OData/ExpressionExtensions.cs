using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryLanguage;

using BinaryExpression = System.Linq.Expressions.BinaryExpression;
using Expression = System.Linq.Expressions.Expression;
using LambdaExpression = Framework.QueryLanguage.LambdaExpression;
using ParameterExpression = Framework.QueryLanguage.ParameterExpression;

namespace Framework.OData
{
    public static class ExpressionExtensions
    {
        public static IEnumerable<Tuple<string, string>> GetPropertyPath(this LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var startParam = expression.Parameters.Single();

            var startProp = ((PropertyExpression)expression.Body);

            var path = startProp.GetAllElements(prop => (prop.Source as ParameterExpression).Maybe(s => s.Name == startParam.Name)
                                                      ? null
                                                      : ((PropertyExpression)prop.Source));

            return path.Reverse().Select(expr => Tuple.Create(expr.PropertyName, (expr as SelectExpression).Maybe(selectExpr => selectExpr.Alias)));
        }
    }



    internal static class StandartExpressionExtensions
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
                IsVirtual = (expr as System.Linq.Expressions.MemberExpression).Maybe(q => virtualNodes.Contains(q))
            });

            var virtualTree2 = virtualTree1.Select((pair, nextChildPairs) =>
            {
                if (pair.Expr is System.Linq.Expressions.LambdaExpression)
                {
                    return new
                    {
                        Expr = pair.Expr,
                        IsVirtual = false
                    };
                }
                else if (pair.Expr.Type == typeof(bool) && (pair.Expr as System.Linq.Expressions.BinaryExpression).Maybe(binExpr => binExpr.NodeType == System.Linq.Expressions.ExpressionType.AndAlso))
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

            var visitor = new OverrideExpressionVisitor(e => e != null && dict[e], System.Linq.Expressions.Expression.Constant(true));

            return filterExpression.UpdateBody(visitor);
        }

        public static bool HasVirtualProperty<T>(this ISelectOrder<T> source)
        {
            return source.Path.HasVirtualProperty();
        }

        public static bool HasVirtualProperty(this System.Linq.Expressions.LambdaExpression expression)
        {
            return expression.GetVirtualChains().Any();
        }

        public static IEnumerable<Stack<MemberExpression>> GetVirtualChains(this System.Linq.Expressions.LambdaExpression expression)
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

    internal static class PropertyInfoExtensions
    {
        private static readonly ReadOnlyCollection<PropertyInfo> SystemVirtualProperties =

              typeof(DateTime).GetProperties(BindingFlags.Instance | BindingFlags.Public).ToReadOnlyCollection();


        private static readonly IDictionaryCache<PropertyInfo, bool> HasSystemOrPrivateFieldCache =

            new DictionaryCache<PropertyInfo, bool>(property => property.IsSystemVirtualProperty() || property.HasPrivateField() || property.HasAttribute<ExpandPathAttribute>()).WithLock();

        private static readonly IDictionaryCache<Type, IEnumerable<Type>> BaseTypesCache = new DictionaryCache<Type, IEnumerable<Type>>(type => type.GetAllElements(q => q.BaseType).TakeWhile(q => typeof(object) != q).ToHashSet(x => x)).WithLock();



        private static bool IsSystemVirtualProperty(this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            if (SystemVirtualProperties.Contains(property))
            {
                return true;
            }

            if (property.DeclaringType.IsNullable())
            {
                return true;
            }

            return false;
        }

        public static bool HasSystemOrPrivateField(this PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return HasSystemOrPrivateFieldCache[property];
        }

        public static bool InSameHierarchy(this Type arg1, Type arg2)
        {
            return BaseTypesCache[arg1].Contains(arg2);
        }
    }
}
