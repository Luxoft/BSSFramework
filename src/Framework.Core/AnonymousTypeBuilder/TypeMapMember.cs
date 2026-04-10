namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMapMember(string name, Type type) : TypeMapMemberBase(name, type), IEquatable<TypeMapMember>
{
    public override int GetHashCode() => this.Name.GetHashCode() ^ this.Type.GetHashCode();

    public bool Equals(TypeMapMember? other) => other != null && this.Name == other.Name && this.Type == other.Type;

    public override bool Equals(object obj) => this.Equals(obj as TypeMapMember);
}
