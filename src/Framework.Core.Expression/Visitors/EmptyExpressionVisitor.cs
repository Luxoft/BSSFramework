using System.Linq.Expressions;

namespace Framework.Core
{
    public class EmptyExpressionVisitor : ExpressionVisitor
    {
        private EmptyExpressionVisitor()
        {
        }

        public static readonly EmptyExpressionVisitor Value = new EmptyExpressionVisitor();
    }
}