using System.Collections.Immutable;

using Framework.Core.ReflectionImpl;
using Framework.Core.TypeResolving.TypeSource;

namespace Framework.Projection;

/// <summary>
/// Для генерации подменяет проекции в памяти на реально скомпилированные проекции в сборке
/// </summary>
public class AlreadyImplementedRuntimeProjectionEnvironment : IProjectionEnvironment
{
    private readonly IProjectionEnvironment baseEnvironment;

    public AlreadyImplementedRuntimeProjectionEnvironment(IProjectionEnvironment baseEnvironment)
    {
        this.baseEnvironment = baseEnvironment ?? throw new ArgumentNullException(nameof(baseEnvironment));

        this.Namespace = this.baseEnvironment.Namespace;
        this.Assembly = new AlreadyImplementedAssemblyInfo(this.baseEnvironment.Assembly);
        this.UseDependencySecurity = this.baseEnvironment.UseDependencySecurity;
    }

    public string Namespace { get; }

    public IAssemblyInfo Assembly { get; }

    public bool UseDependencySecurity { get; }

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
