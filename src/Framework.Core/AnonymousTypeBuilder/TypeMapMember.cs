namespace Framework.Core;

public class TypeMapMember(string name, Type type) : TypeMapMemberBase(name, type), IEquatable<TypeMapMember>
{
    public override int GetHashCode()
    {
        return this.Name.GetHashCode() ^ this.Type.GetHashCode();
    }

    public bool Equals(TypeMapMember other)
    {
        return other != null && this.Name == other.Name && this.Type == other.Type;
    }


    public override bool Equals(object obj)
    {
        return this.Equals(obj as TypeMapMember);
    }
}
