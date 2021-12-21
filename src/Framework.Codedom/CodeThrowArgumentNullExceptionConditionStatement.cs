using System;
using System.CodeDom;

namespace Framework.CodeDom
{
    public class CodeThrowArgumentNullExceptionConditionStatement : CodeConditionStatement
    {
        public CodeThrowArgumentNullExceptionConditionStatement(CodeParameterDeclarationExpression parameter)
            : base(new CodeIsNullExpression(parameter.ToVariableReferenceExpression()),
                   new CodeThrowExceptionStatement(typeof(ArgumentNullException).ToTypeReference()
                                                       .ToObjectCreateExpression(new CodePrimitiveExpression(parameter.Name))))
        {

        }
    }
}