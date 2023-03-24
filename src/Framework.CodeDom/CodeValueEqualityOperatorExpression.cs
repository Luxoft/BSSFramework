using System.CodeDom;

namespace Framework.CodeDom;

public class CodeValueEqualityOperatorExpression : CodeBinaryOperatorExpression
{
    public CodeValueEqualityOperatorExpression(CodeExpression left, CodeExpression right)
            : base (left, CodeBinaryOperatorType.ValueEquality, right)
    {

    }
}
