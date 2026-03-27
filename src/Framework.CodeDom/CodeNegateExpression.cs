using System.CodeDom;

namespace Framework.CodeDom;

public class CodeNegateExpression(CodeExpression value) : CodeValueEqualityOperatorExpression(value, new CodePrimitiveExpression(false));
