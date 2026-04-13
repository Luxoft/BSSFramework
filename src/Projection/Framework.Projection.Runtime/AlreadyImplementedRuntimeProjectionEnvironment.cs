using System.Reflection;

using Framework.Core.ReflectionImpl;
using Framework.ExtendedMetadata;

namespace Framework.Projection;

/// <summary>
/// Для генерации подменяет проекции в памяти на реально скомпилированные проекции в сборке
/// </summary>
public class AlreadyImplementedRuntimeProjectionEnvironment(IProjectionEnvironment baseEnvironment) : IProjectionEnvironment
{
    public string Namespace { get; } = baseEnvironment.Namespace;

    public Assembly Assembly { get; } = new AlreadyImplementedAssembly(baseEnvironment.Assembly);

    public bool UseDependencySecurity { get; } = baseEnvironment.UseDependencySecurity;

    public IMetadataProxyProvider MetadataProxyProvider { get; } = baseEnvironment.MetadataProxyProvider;

    private class AlreadyImplementedAssembly(Assembly baseAssembly) : Assembly
    {
        //public string Name => baseAssembly.Name;

        public override string? FullName => baseAssembly.FullName;

        public override Type[] GetTypes() =>
        [
            .. baseAssembly.GetTypes().Select(baseType =>
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
