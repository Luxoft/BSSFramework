using System.Collections.Immutable;

using Framework.Core.ReflectionImpl;
using Framework.Core.TypeResolving.TypeSource;
using Framework.ExtendedMetadata;

namespace Framework.Projection;

/// <summary>
/// Для генерации подменяет проекции в памяти на реально скомпилированные проекции в сборке
/// </summary>
public class AlreadyImplementedRuntimeProjectionEnvironment(IProjectionEnvironment baseEnvironment) : IProjectionEnvironment
{
    public string Namespace { get; } = baseEnvironment.Namespace;

    public IAssemblyInfo Assembly { get; } = new AlreadyImplementedAssemblyInfo(baseEnvironment.Assembly);

    public bool UseDependencySecurity { get; } = baseEnvironment.UseDependencySecurity;

    public IMetadataProxyProvider MetadataProxyProvider { get; } = baseEnvironment.MetadataProxyProvider;

    private class AlreadyImplementedAssemblyInfo(IAssemblyInfo baseAssembly) : IAssemblyInfo
    {
        public string Name => baseAssembly.Name;

        public string FullName => baseAssembly.FullName;

        public ImmutableHashSet<Type> Types { get; } =
        [
            .. baseAssembly.Types.Select(baseType =>
            {
                if (baseType is BaseTypeImpl genType)
                {
                    return genType.TryGetRealType() ?? baseType;
                }
                else
                {
                    return baseType;
                }
            })
        ];
    }
}
