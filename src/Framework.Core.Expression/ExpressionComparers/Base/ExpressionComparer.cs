using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal abstract class ExpressionComparer<TExpression> : IEqualityComparer<TExpression>
        where TExpression : Expression
    {
        public virtual bool Equals(TExpression x, TExpression y)
        {
            return x.NodeType == y.NodeType && x.Type == y.Type;
        }

        public virtual int GetHashCode(TExpression obj)
        {
            return obj.Type.GetHashCode() ^ obj.NodeType.GetHashCode();
        }
    }

    public class ExpressionComparer : IEqualityComparer<Expression>
    {
        private ExpressionComparer()
        {

        }

        public bool Equals(Expression x, Expression y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (x != null && y != null && x.NodeType == y.NodeType && x.GetType() == y.GetType())
            {
                if (x is LambdaExpression && y is LambdaExpression)
                {
                    return LambdaComparer.Value.Equals(x as LambdaExpression, y as LambdaExpression);
                }

                if (x is MethodCallExpression && y is MethodCallExpression)
                {
                    return MethodCallComparer.Value.Equals(x as MethodCallExpression, y as MethodCallExpression);
                }

                if (x is MemberExpression && y is MemberExpression)
                {
                    return MemberComparer.Value.Equals(x as MemberExpression, y as MemberExpression);
                }

                if (x is BinaryExpression && y is BinaryExpression)
                {
                    return BinaryComparer.Value.Equals(x as BinaryExpression, y as BinaryExpression);
                }

                if (x is ParameterExpression && y is ParameterExpression)
                {
                    return ParameterComparer.Value.Equals(x as ParameterExpression, y as ParameterExpression);
                }

                if (x is ConstantExpression && y is ConstantExpression)
                {
                    return ConstantComparer.Value.Equals(x as ConstantExpression, y as ConstantExpression);
                }

                if (x is UnaryExpression && y is UnaryExpression)
                {
                    return UnaryComparer.Value.Equals(x as UnaryExpression, y as UnaryExpression);
                }

                if (x is NewArrayExpression && y is NewArrayExpression)
                {
                    return NewArrayComparer.Value.Equals(x as NewArrayExpression, y as NewArrayExpression);
                }

                if (x is NewExpression && y is NewExpression)
                {
                    return NewComparer.Value.Equals(x as NewExpression, y as NewExpression);
                }

                if (x is MemberInitExpression && y is MemberInitExpression)
                {
                    return MemberInitComparer.Value.Equals(x as MemberInitExpression, y as MemberInitExpression);
                }

                if (x is ConditionalExpression && y is ConditionalExpression)
                {
                    return ConditionalComparer.Value.Equals(x as ConditionalExpression, y as ConditionalExpression);
                }

                if (x is ListInitExpression && y is ListInitExpression)
                {
                    return ListInitComparer.Value.Equals(x as ListInitExpression, y as ListInitExpression);
                }

                throw new NotImplementedException($"Can not implement {x.GetType().Name} expression compare");
            }

            return false;
        }


        public int GetHashCode(Expression obj)
        {
            return obj.Maybe(v => v.Type.GetHashCode() ^ v.NodeType.GetHashCode());
        }


        public static readonly ExpressionComparer Value = new ExpressionComparer();
    }
}