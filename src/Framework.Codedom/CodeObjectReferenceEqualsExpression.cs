using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeObjectReferenceEqualsExpression : CodeMethodInvokeExpression
    {
        public CodeObjectReferenceEqualsExpression(CodeExpression value1, CodeExpression value2)
            : base(new CodeTypeReferenceExpression(typeof(object)), "ReferenceEquals", value1, value2)
        {

        }
    }
}