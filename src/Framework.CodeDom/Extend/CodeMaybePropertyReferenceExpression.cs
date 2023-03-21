using System.CodeDom;

namespace Framework.CodeDom;

public class CodeMaybePropertyReferenceExpression : CodePropertyReferenceExpression
{
    public CodeMaybePropertyReferenceExpression()
    {
    }

    public CodeMaybePropertyReferenceExpression(CodeExpression targetObject, string propertyName)
            : base(targetObject, propertyName)
    {
    }
}
