using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeIsNotNullExpression : CodeNegateExpression
    {
        public CodeIsNotNullExpression(CodeExpression value) : base(new CodeIsNullExpression(value))
        {

        }
    }
}