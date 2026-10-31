using System.Reflection;

using Framework.ExtendedMetadata;

namespace Framework.Projection;

/// <summary>
/// Projection environment
/// </summary>
public interface IProjectionEnvironment
{
    /// <summary>
    /// Namespace
    /// </summary>
    string Namespace { get; }

    /// <summary>
    /// Assembly
    /// </summary>
    Assembly Assembly { get; }

    /// <summary>
    /// Use of security through the `DependencySecurityAttribute` attribute
    /// </summary>
    bool UseDependencySecurity { get; }

    IMetadataProxyProvider MetadataProxyProvider { get; }
}
