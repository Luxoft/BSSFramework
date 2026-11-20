using System.Reflection;

namespace Framework.Core.ReflectionImpl;

public static class PropertyNullabilityExtensions
{
    /// <summary>
    /// Reads the compiler-emitted nullable metadata (NullableContext/Nullable attributes) of a reference-type property
    /// </summary>
    public static bool IsNullableReference(this PropertyInfo property)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        // Synthetic (projection) properties carry their own nullability
        if (property is BasePropertyInfoImpl synthetic)
        {
            return synthetic.IsNullableReferenceType;
        }

        if (property.PropertyType.IsValueType)
        {
            return false;
        }

        var info = new NullabilityInfoContext().Create(property);

        return info.ReadState == NullabilityState.Nullable || info.WriteState == NullabilityState.Nullable;
    }

    /// <summary>
    /// The path result is a nullable reference: the last property is a reference type and any node of the path may be null
    /// </summary>
    /// <param name="path">Property path</param>
    /// <param name="isCollectionResult">The result is a collection: a collection reached through a multi-node path is never null (empty instead)</param>
    public static bool IsNullableReferenceResult(this PropertyPath path, bool isCollectionResult = false)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        if (isCollectionResult && path.Count > 1)
        {
            return false;
        }

        return path.Any() && !path.Last().PropertyType.IsValueType && path.Any(prop => prop.IsNullableReference());
    }
}
