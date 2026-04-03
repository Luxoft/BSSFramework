namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMapMemberBase : ITypeMapMember
{
    public TypeMapMemberBase(string name, Type type)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Name = name;
        this.Type = type;
    }


    public string Name { get; }

    public Type Type { get; }


    public override string ToString() => this.Name;
}
