using System.CodeDom;

using Framework.CodeDom.Extensions;

namespace Framework.CodeDom;

public class CodeThrowInvalidOperationThrowExceptionStatement(string message)
    : CodeThrowExceptionStatement(typeof(InvalidOperationException).ToTypeReference().ToObjectCreateExpression(new CodePrimitiveExpression(message)));
