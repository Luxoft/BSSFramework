using System.CodeDom;

namespace Framework.CodeDom;

public class CodeNullConditionStatement(CodeExpression expression) : CodeConditionStatement(new CodeIsNullExpression(expression));
