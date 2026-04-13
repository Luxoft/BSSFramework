using System.Collections.Immutable;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Framework.Core.TypeResolving;

public class TypeSource(ImmutableHashSet<Type> types) : ITypeSource
{
    public TypeSource(IEnumerable<Assembly> assemblies)
        : this([.. assemblies.SelectMany(assembly => assembly.GetTypes())])
    {
    }

    public TypeSource(IEnumerable<AppDomain> domains)
        : this(domains.SelectMany(domain => domain.GetAssemblies()))
    {
    }

    public ImmutableHashSet<Type> Types => types;

    public static TypeSource FromSample(Type sampleType) => new([sampleType.Assembly]);

    public static TypeSource FromSample<T>() => FromSample(typeof(T));
}
