using System.CodeDom;

using Framework.CodeDom.Extensions;

namespace Framework.CodeDom;

public class CodeThrowArgumentOutOfRangeExceptionStatement(string parameterName) : CodeThrowExceptionStatement(
    typeof(ArgumentOutOfRangeException).ToTypeReference().ToObjectCreateExpression(new CodePrimitiveExpression(parameterName)))
{
    public CodeThrowArgumentOutOfRangeExceptionStatement(CodeParameterDeclarationExpression parameter)
            : this(parameter.Name)
    {
    }

    public CodeThrowArgumentOutOfRangeExceptionStatement(CodeTypeParameter parameter)
            : this(parameter.Name)
    {
    }
}
