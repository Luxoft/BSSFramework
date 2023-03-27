using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core;

public class TypeSource : ITypeSource
{
    private readonly IEnumerable<Type> _types;


    public TypeSource([NotNull] IEnumerable<Type> types)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));

        this._types = types;
    }


    public TypeSource([NotNull] IEnumerable<Assembly> assemblies)
            : this(assemblies.SelectMany(assembly => assembly.GetTypes()))
    {

    }

    public TypeSource(IEnumerable<AppDomain> domains)
            : this(domains.SelectMany(domain => domain.GetAssemblies()))
    {

    }

    public TypeSource([NotNull] params Type[] types)
            : this((IEnumerable<Type>)types)
    {

    }

    public TypeSource([NotNull] params Assembly[] assemblies)
            : this((IEnumerable<Assembly>)assemblies)
    {

    }

    public TypeSource(params AppDomain[] domains)
            : this((IEnumerable<AppDomain>) domains)
    {

    }



    public IEnumerable<Type> GetTypes()
    {
        return this._types;
    }



    public static TypeSource FromSample([NotNull] Type sampleType)
    {
        if (sampleType == null) throw new ArgumentNullException(nameof(sampleType));

        return new TypeSource(sampleType.Assembly);
    }

    public static TypeSource FromSample<T>()
    {
        return FromSample(typeof(T));
    }


    public static readonly TypeSource CurrentDomain = new TypeSource(AppDomain.CurrentDomain);
}
