using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.Core.ReflectionImpl;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class PropertyNullabilityExtensions
{
    /// <summary>
    /// The <c>null</c> literal to assign to a property: <c>null!</c> when the property is not nullable (runtime value is the same)
    /// </summary>
    public static CodeExpression ToNullValueExpression(this PropertyInfo property) =>
        property.IsNullableReference() ? new CodePrimitiveExpression(null) : new CodePrimitiveExpression(null).ToNullForgivingExpression();
}
