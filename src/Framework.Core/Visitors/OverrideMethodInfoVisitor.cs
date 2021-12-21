using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public class OverrideMethodInfoVisitor : ExpressionVisitor
    {
        private readonly MethodInfo _methodInfo;
        private readonly LambdaExpression _expression;


        public OverrideMethodInfoVisitor(MethodInfo methodInfo, LambdaExpression expression)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            if (methodInfo.GetParameters().Length != expression.Parameters.Count) throw new Exception("Different parameters count");

            this._methodInfo = methodInfo;
            this._expression = expression;
        }


        protected override Expression VisitUnary(UnaryExpression node)
        {
            return node.Method == this._methodInfo ? this.GetExpressionByArgs(new[] { node.Operand }) : base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            return node.Method == this._methodInfo ? this.GetExpressionByArgs(new[] { node.Left, node.Right }) : base.VisitBinary(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return node.Method == this._methodInfo ? this.GetExpressionByArgs(node.Arguments) : base.VisitMethodCall(node);
        }


        private Expression GetExpressionByArgs(IEnumerable<Expression> arguments)
        {
            if (arguments == null) throw new ArgumentNullException(nameof(arguments));

            return this._expression
                    .Parameters
                    .Zip(arguments, (parameter, argument) => new { Parameter = parameter, Argument = argument })
                    .Aggregate(this._expression.Body, (state, pair) => state.Override(pair.Parameter, pair.Argument));
        }
    }

    public class OverrideMethodInfoVisitor<TDelegate> : OverrideMethodInfoVisitor
        //where TDelegate : Delegate
    {
        public OverrideMethodInfoVisitor(TDelegate func, Expression<TDelegate> expression)
            : base(((Delegate)(object)func).Method, expression)
        {

        }
    }
}