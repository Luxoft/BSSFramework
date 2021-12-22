using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeNegateExpression : CodeValueEqualityOperatorExpression
    {
        public CodeNegateExpression(CodeExpression value)
            : base(value, new CodePrimitiveExpression(false))
        {

        }
    }
}