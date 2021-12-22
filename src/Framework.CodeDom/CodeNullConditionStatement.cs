using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeNullConditionStatement : CodeConditionStatement
    {
        public CodeNullConditionStatement(CodeExpression expression)
            : base(new CodeIsNullExpression(expression))
        {

        }
    }
}