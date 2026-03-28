using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.CodeDom.Extend;
using Framework.Core;

namespace Framework.CodeDom.Extensions;

public static class CodeExpressionExtensions
{
    public static bool IsPrimitiveValue<T>(this CodeExpression expression, T value) => (expression as CodePrimitiveExpression).Maybe(expr => (expr.Value is T argValue) && EqualityComparer<T>.Default.Equals(argValue, value));

    public static CodeExpression ToAsCastExpression(this CodeExpression expression, CodeTypeReference targetType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (targetType == null) throw new ArgumentNullException(nameof(targetType));

        var genericAsCastMethod = new Func<object, object>(CorePipeObjectExtensions.AsCast<object>).Method.GetGenericMethodDefinition();

        var refMethod = new CodeMethodReferenceExpression(
                                                          new CodeTypeReferenceExpression(genericAsCastMethod.DeclaringType),
                                                          genericAsCastMethod.Name,
                                                          targetType);

        return new CodeMethodInvokeExpression(refMethod, expression);
    }

    public static CodeExpression ToCastExpression(this CodeExpression expression, CodeTypeReference targetType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (targetType == null) throw new ArgumentNullException(nameof(targetType));

        return new CodeCastExpression(targetType, expression);
    }

    public static CodeExpression ToCastCollectionExpression(this CodeExpression expression, CodeTypeReference targetType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (targetType == null) throw new ArgumentNullException(nameof(targetType));

        return expression.ToStaticMethodInvokeExpression(typeof(Enumerable).ToTypeReferenceExpression().ToMethodReferenceExpression("Cast", targetType));
    }

    public static CodeExpression ToMethodToStringExpression(this CodeExpression codeExpression)
    {
        if (codeExpression == null) throw new ArgumentNullException(nameof(codeExpression));

        return codeExpression.ToMethodInvokeExpression("ToString");
    }

    public static CodeStatementCollection ToStatementCollection(this IEnumerable<CodeStatement> codeStatements)
    {
        if (codeStatements == null) throw new ArgumentNullException(nameof(codeStatements));

        return new CodeStatementCollection(codeStatements.ToArray());
    }

    public static CodeLambdaExpression ToCodeLambdaExpression(this PropertyInfo property, string varName = "source")
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (string.IsNullOrWhiteSpace(varName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(varName));

        var varDecl = new CodeParameterDeclarationExpression { Name = varName };

        return new CodeLambdaExpression
               {
                       Parameters = { varDecl },
                       Statements = { varDecl.ToVariableReferenceExpression().ToPropertyReference(property) }
               };
    }
}
