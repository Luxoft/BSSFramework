using System.CodeDom;

namespace Framework.CodeDom;

public class CodeObjectEqualsExpression : CodeMethodInvokeExpression
{
    public CodeObjectEqualsExpression(CodeExpression value1, CodeExpression value2)
            : base(new CodeTypeReferenceExpression(typeof(object)), "Equals", value1, value2)
    {

    }
}
