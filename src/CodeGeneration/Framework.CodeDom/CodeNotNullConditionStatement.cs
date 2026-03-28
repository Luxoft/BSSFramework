using System.CodeDom;

namespace Framework.CodeDom;

public class CodeNotNullConditionStatement(CodeExpression expression) : CodeConditionStatement(new CodeIsNotNullExpression(expression));
