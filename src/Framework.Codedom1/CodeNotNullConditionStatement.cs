using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeNotNullConditionStatement : CodeConditionStatement
    {
        public CodeNotNullConditionStatement (CodeExpression expression)
            : base(new CodeIsNotNullExpression(expression))
        {

        }
    }
}