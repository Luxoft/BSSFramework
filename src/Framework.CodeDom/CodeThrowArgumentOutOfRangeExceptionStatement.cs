using System.CodeDom;

namespace Framework.CodeDom;

public class CodeThrowArgumentOutOfRangeExceptionStatement : CodeThrowExceptionStatement
{
    public CodeThrowArgumentOutOfRangeExceptionStatement(string parameterName)
            : base(typeof(ArgumentOutOfRangeException).ToTypeReference().ToObjectCreateExpression(new CodePrimitiveExpression(parameterName)))
    {
    }

    public CodeThrowArgumentOutOfRangeExceptionStatement(CodeParameterDeclarationExpression parameter)
            : this(parameter.Name)
    {
    }

    public CodeThrowArgumentOutOfRangeExceptionStatement(CodeTypeParameter parameter)
            : this(parameter.Name)
    {
    }
}
