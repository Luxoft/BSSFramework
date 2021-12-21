using System;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.CustomReports.Services
{
    internal class ReplaceExpressionVisistor<TFrom, TResult> : ExpressionVisitor
    {
        private readonly Type _toType;
        private readonly Tuple<string, string>[] _replaceProperty;

        private Expression nextParameter;

        public ReplaceExpressionVisistor(Type toType, params Tuple<string, string>[] replaceProperty)
        {
            this._toType = toType;
            this._replaceProperty = replaceProperty;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.MemberType == System.Reflection.MemberTypes.Property)
            {
                var tryReplaced = this._replaceProperty.FirstOrDefault(z => string.Equals(node.Member.Name, z.Item1, StringComparison.CurrentCultureIgnoreCase));
                if (tryReplaced != null)
                {
                    var nextExpression = base.Visit(node.Expression);
                    return Expression.Property(nextExpression, this._toType, tryReplaced.Item2);
                }
            }
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == typeof(TFrom))
            {
                return this.nextParameter ?? (this.nextParameter = Expression.Parameter(this._toType, node.Name));
            }
            return base.VisitParameter(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T) == typeof(Func<TFrom, TResult>))
            {
                var getLambdaMethod = (Func<Expression<T>, Expression>) (this.GetLambda2<T, object>);

                return (Expression) getLambdaMethod.CreateGenericMethod(new[]
                {
                    typeof(T),
                    this._toType
                }).Invoke(this, new[] {node});
            }

            return base.VisitLambda(node);
        }

        private Expression GetLambda2<T, TTo>(Expression<T> node)
        {
            return Expression.Lambda<Func<TTo, TResult>>(this.Visit(node.Body), node.Parameters.Select(this.VisitParameter).Cast<ParameterExpression>());
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }
    }
}
