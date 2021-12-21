using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeValueUnequalityOperatorExpression : CodeBinaryOperatorExpression
    {
        public CodeValueUnequalityOperatorExpression(CodeExpression left, CodeExpression right)
            : base(new CodeValueEqualityOperatorExpression(left, right), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false))
        {

        }
    }
}