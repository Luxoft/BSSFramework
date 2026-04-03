using System.CodeDom;

namespace Framework.CodeDom;

public class CodeValueUnequalityOperatorExpression(CodeExpression left, CodeExpression right) : CodeBinaryOperatorExpression(
    new CodeValueEqualityOperatorExpression(left, right),
    CodeBinaryOperatorType.ValueEquality,
    new CodePrimitiveExpression(false));
