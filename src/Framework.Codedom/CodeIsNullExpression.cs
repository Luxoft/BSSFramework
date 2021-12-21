using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeIsNullExpression : CodeMethodInvokeExpression
    {
        public CodeIsNullExpression(CodeExpression value)
            : base(new CodeTypeReferenceExpression(typeof(object)), "ReferenceEquals", value, new CodePrimitiveExpression(null))
        {

        }
    }
}