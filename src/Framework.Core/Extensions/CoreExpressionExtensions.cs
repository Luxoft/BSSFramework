using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class CoreExpressionExtensions
    {
        public static PropertyPath ToPropertyPath([NotNull] this LambdaExpression source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.GetReverseProperties().Reverse().ToPropertyPath();
        }

        public static Node<Expression> ToNode(this Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var visitor = new NodeExpressionVisitor(expression);

            visitor.Visit(expression);

            return visitor.ToNode();
        }

        private class NodeExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _startNode;

            private readonly List<NodeExpressionVisitor> ChildVisitors = new List<NodeExpressionVisitor>();

            public NodeExpressionVisitor(Expression startNode)
            {
                if (startNode == null) throw new ArgumentNullException(nameof(startNode));

                this._startNode = startNode;
            }

            public override Expression Visit(Expression node)
            {
                if (node == null || node == this._startNode)
                {
                    return base.Visit(node);
                }
                else
                {
                    var childVisitor = new NodeExpressionVisitor(node);
                    this.ChildVisitors.Add(childVisitor);
                    return childVisitor.Visit(node);
                }
            }

            public Node<Expression> ToNode()
            {
                return new Node<Expression>(this._startNode, this.ChildVisitors.Select(child => child.ToNode()));
            }
        }


        public static string GetMemberName<TSource, TResult>(this Expression<Func<TSource, TResult>> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return expr.Body.GetMember().Select(member => member.Name)
                                        .GetValue(() => new System.ArgumentException("not member expression", nameof(expr)));
        }

        public static PropertyInfo GetProperty<TSource, TResult>(this Expression<Func<TSource, TResult>> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            var request = from member in expr.Body.GetMember()

                          from property in (member as PropertyInfo).ToMaybe()

                          select property;

            return request.GetValue(() => new System.ArgumentException("not property expression", nameof(expr)));
        }

        /// <summary>
        /// Получение полного пути из Expression
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static PropertyPath GetPropertyPath<TSource, TResult>(this Expression<Func<TSource, TResult>> expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return expr.GetReverseProperties().Reverse().ToPropertyPath();
        }


        private static IEnumerable<PropertyInfo> GetReverseProperties<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            var parameter = expression.Parameters[0];

            var current = expression.Body;

            while (current != parameter)
            {
                var memberExpr = (MemberExpression)current;

                var property = (PropertyInfo)memberExpr.Member;

                yield return property;

                current = memberExpr.Expression;
            }
        }

        private static Maybe<MemberInfo> GetMember(this Expression expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));

            return (expr as UnaryExpression).ToMaybe().Where(unaryExpr => unaryExpr.NodeType == ExpressionType.Convert)
                                                            .SelectMany(unaryExpr => unaryExpr.Operand.GetMember())

         .Or(() => (expr as MethodCallExpression).ToMaybe().Select(callExpr => (MemberInfo)callExpr.Method))

         .Or(() => (expr as MemberExpression).ToMaybe().Select(memberExpr => memberExpr.Member));
        }
    }
}
