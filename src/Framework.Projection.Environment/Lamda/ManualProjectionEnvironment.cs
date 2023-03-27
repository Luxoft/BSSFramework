using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Metadata;

namespace Framework.Projection.Lambda;

public class ManualProjectionEnvironment : IProjectionEnvironment
{
    public ManualProjectionEnvironment(Assembly assembly, Type persistentDomainObjectBaseType)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));
        if (persistentDomainObjectBaseType == null) throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));

        this.Assembly = AssemblyInfo.Create(assembly, persistentDomainObjectBaseType.IsAssignableFrom);
    }

    public string Namespace => throw new NotImplementedException("Single namespace not required");

    public IAssemblyInfo Assembly { get; }

    public bool UseDependencySecurity { get; } = true;
}
