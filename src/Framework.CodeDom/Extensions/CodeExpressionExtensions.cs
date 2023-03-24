using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.CodeDom;

public static class CodeExpressionExtensions
{
    public static bool IsPrimitiveValue<T>(this CodeExpression expression, T value)
    {
        return (expression as CodePrimitiveExpression).Maybe(expr => (expr.Value is T) && EqualityComparer<T>.Default.Equals((T)expr.Value, value));
    }

    public static CodeExpression ToAsCastExpression(this CodeExpression expression, CodeTypeReference targetType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (targetType == null) throw new ArgumentNullException(nameof(targetType));

        var genericAsCastMethod = new Func<object, object>(PipeObjectExtensions.AsCast<object>).Method.GetGenericMethodDefinition();

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

    public static CodeStatementCollection ToStatementCollection([NotNull] this IEnumerable<CodeStatement> codeStatements)
    {
        if (codeStatements == null) throw new ArgumentNullException(nameof(codeStatements));

        return new CodeStatementCollection(codeStatements.ToArray());
    }

    public static CodeLambdaExpression ToCodeLambdaExpression([NotNull] this PropertyInfo property, [NotNull] string varName = "source")
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
