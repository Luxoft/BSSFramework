using Framework.Core;
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
        public IEnumerable<Type> GetTypes()
        {
            foreach (var baseType in baseAssembly.GetTypes())
            {
                if (baseType is BaseTypeImpl genType)
                {
                    yield return genType.TryGetRealType() ?? baseType;
                }
                else
                {
                    yield return baseType;
                }
            }
        }

        public string Name => baseAssembly.Name;

        public string FullName => baseAssembly.FullName;
    }
}
