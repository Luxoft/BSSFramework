using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.Projection.Lambda;

/// <summary>
/// Projection property interface
/// </summary>
public interface IProjectionProperty : IProjectionAttributeProvider
{
    /// <summary>
    /// Property name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Property role
    /// </summary>
    ProjectionPropertyRole Role { get; }

    /// <summary>
    /// Expression path to the property
    /// </summary>
    LambdaExpression Expression { get; }

    /// <summary>
    /// Property path to the property
    /// </summary>
    PropertyPath Path { get; }

    /// <summary>
    /// Source type from which the projection is built
    /// </summary>
    Type SourceType { get; }

    /// <summary>
    /// Projection property type
    /// </summary>
    TypeReferenceBase.BuildTypeReference Type { get; }

    /// <summary>
    /// Disable serialization of the property in DTO
    /// </summary>
    bool IgnoreSerialization { get; }

    /// <summary>
    /// Virtual interface property implemented explicitly
    /// </summary>
    PropertyInfo? VirtualExplicitInterfaceProperty { get; }
}
