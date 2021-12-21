using System;

namespace Framework.Core
{
    public class TypeMapMember : TypeMapMemberBase, IEquatable<TypeMapMember>
    {
        public TypeMapMember(string name, Type type) : base(name, type)
        {

        }

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
}