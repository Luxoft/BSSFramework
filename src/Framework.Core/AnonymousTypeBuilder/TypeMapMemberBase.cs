namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMapMemberBase(string name, Type type) : ITypeMapMember
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public override string ToString() => this.Name;
}
