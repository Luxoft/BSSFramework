using System.CodeDom;

namespace Framework.CodeDom;

public class CodeThrowInvalidOperationThrowExceptionStatement : CodeThrowExceptionStatement
{
    public CodeThrowInvalidOperationThrowExceptionStatement(string message)
            : base(typeof(InvalidOperationException).ToTypeReference().ToObjectCreateExpression(new CodePrimitiveExpression(message)))
    {

    }
}
