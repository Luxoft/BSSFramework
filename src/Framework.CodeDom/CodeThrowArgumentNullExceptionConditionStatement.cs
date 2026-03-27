using System.CodeDom;

using Framework.CodeDom.Extensions;

namespace Framework.CodeDom;

public class CodeThrowArgumentNullExceptionConditionStatement(CodeParameterDeclarationExpression parameter) : CodeConditionStatement(
    new CodeIsNullExpression(parameter.ToVariableReferenceExpression()),
    new CodeThrowExceptionStatement(
        typeof(ArgumentNullException).ToTypeReference()
                                     .ToObjectCreateExpression(new CodePrimitiveExpression(parameter.Name))));
