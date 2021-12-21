using System.CodeDom;

namespace Framework.CodeDom.TypeScript
{
    /// <summary>
    /// Throw error condition statement if input parameteter is null or indefined
    /// </summary>
    public class CodeThrowArgumentIsNullOrUndefinedExceptionConditionStatement : CodeConditionStatement
    {
        public CodeThrowArgumentIsNullOrUndefinedExceptionConditionStatement(CodeParameterDeclarationExpression parameter)
            : base(new CodeSnippetExpression(parameter.Name).ToIsNullOrUndefinedExpression(),
                  new CodeThrowExceptionStatement(new CodeTypeReference("Error").ToObjectCreateExpression(new CodePrimitiveExpression($"Parameter {parameter.Name} should be specified"))))
        {
        }
    }
}
