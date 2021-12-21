using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeBooleanAndOperatorExpression : CodeBinaryOperatorCollectionExpression
    {
        public CodeBooleanAndOperatorExpression(params CodeExpression[] operations)
            : base(CodeBinaryOperatorType.BooleanAnd, operations)
        {
        }
    }

    public class CodeBooleanOrOperatorExpression : CodeBinaryOperatorCollectionExpression
    {
        public CodeBooleanOrOperatorExpression(params CodeExpression[] operations)
            : base(CodeBinaryOperatorType.BooleanOr, operations)
        {
        }
    }
}