using System;
using Framework.Core;

namespace Framework.Workflow.BLL
{
    public class ParameterizedTypeMapMember : TypeMapMember, IEquatable<ParameterizedTypeMapMember>
    {
        public ParameterizedTypeMapMember(string name, Type type, bool allowNull)
            : base(name, type)
        {
            this.AllowNull = allowNull;
        }


        public bool AllowNull { get; private set; }


        public bool Equals(ParameterizedTypeMapMember other)
        {
            return base.Equals(other) && this.AllowNull == other.AllowNull;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ParameterizedTypeMapMember);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ this.AllowNull.GetHashCode();
        }
    }
}