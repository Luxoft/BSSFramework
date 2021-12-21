using System;
using System.Linq.Expressions;

namespace Framework.Core
{
    public class OverriteBooleanExpressionVisitor<TSource, TTarget> : ExpressionVisitor
    {
        private readonly Expression<Func<TTarget, TSource>> _sourceExpression;
        private readonly ParameterExpression _parameterExpression;

        public OverriteBooleanExpressionVisitor(Expression<Func<TTarget, TSource>> sourceExpression)
        {
            this._parameterExpression = Expression.Parameter(typeof(TTarget), "z");
            this._sourceExpression = (Expression<Func<TTarget, TSource>>)new OverrideParameterExpressionVisitor(this._parameterExpression).Visit(sourceExpression);

        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this._parameterExpression;
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType == typeof(TSource))
            {
                return Expression.Property(this._sourceExpression.Body, node.Member.Name);
            }
            return base.VisitMember(node);
        }
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda<Func<TTarget, bool>>(this.Visit(node.Body), this._parameterExpression);
        }
        public class OverrideParameterExpressionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _targetParameterExpression;

            public OverrideParameterExpressionVisitor(ParameterExpression targetParameterExpression)
            {
                this._targetParameterExpression = targetParameterExpression;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return this._targetParameterExpression;
            }
        }
    }
}