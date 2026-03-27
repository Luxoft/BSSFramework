using System.CodeDom;

namespace Framework.CodeDom;

public class CodeValueEqualityOperatorExpression(CodeExpression left, CodeExpression right) : CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.ValueEquality, right);
