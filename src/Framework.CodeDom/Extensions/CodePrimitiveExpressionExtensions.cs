using System.CodeDom;

using Framework.Core;

namespace Framework.CodeDom;

public static class CodePrimitiveExpressionExtensions
{
    public static CodeExpression ToDynamicPrimitiveExpression(this object value)
    {
        return (value as Enum).Maybe(e => e.ToPrimitiveExpression(), new CodePrimitiveExpression(value));
    }

    public static CodeExpression ToPrimitiveExpression(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return value.ToString()
                    .Split(new[] { ", " }, StringSplitOptions.None)
                    .Select(flag => value.GetType().ToTypeReferenceExpression().ToFieldReference(flag))
                    .Aggregate<CodeExpression>((v1, v2) => new CodeBinaryOperatorExpression(v1, CodeBinaryOperatorType.BitwiseOr, v2));
    }

    public static CodeExpression ToPrimitiveExpression(this bool value)
    {
        return new CodePrimitiveExpression(value);
    }

    public static CodeExpression ToPrimitiveExpression(this int value)
    {
        return new CodePrimitiveExpression(value);
    }

    public static CodeExpression ToPrimitiveExpression(this long value)
    {
        return new CodePrimitiveExpression(value);
    }

    public static CodeExpression ToPrimitiveExpression(this string value)
    {
        return new CodePrimitiveExpression(value);
    }
}
