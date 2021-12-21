using System;

namespace Framework.Core
{
    public class TypeMapMemberBase : ITypeMapMember
    {
        public TypeMapMemberBase(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));

            this.Name = name;
            this.Type = type;
        }


        public string Name { get; private set; }

        public Type Type { get; private set; }


        public override string ToString()
        {
            return this.Name;
        }
    }
}