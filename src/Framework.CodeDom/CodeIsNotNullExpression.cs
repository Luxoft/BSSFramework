using System.CodeDom;

namespace Framework.CodeDom;

public class CodeIsNotNullExpression(CodeExpression value) : CodeNegateExpression(new CodeIsNullExpression(value));
