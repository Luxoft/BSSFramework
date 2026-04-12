using System.Collections.Immutable;
using System.Reflection;

using Framework.Core.TypeResolving;

namespace Framework.Database.Metadata;

public class AssemblyInfo : IAssemblyInfo
{
    private readonly ITypeSource typeSource;

    public AssemblyInfo(string name, string fullName, ITypeSource typeSource)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(fullName));
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        this.Name = name;
        this.FullName = fullName;
        this.typeSource = typeSource;
    }

    public string Name { get; }

    public string FullName { get; }


    public ImmutableHashSet<Type> Types => this.typeSource.Types;

    public static AssemblyInfo Create(Assembly assembly, Func<Type, bool>? typeFilter = null)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));

        var typeSource = typeFilter == null ? new TypeSource([assembly]) : new TypeSource([..assembly.GetTypes().Where(typeFilter)]);

        return new AssemblyInfo(assembly.GetName().Name!, assembly.FullName!, typeSource);
    }
}
