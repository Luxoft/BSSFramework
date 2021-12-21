using System;
using System.Runtime.Serialization;

namespace Framework.DomainDriven.SerializeMetadata
{
    [DataContract]
    public enum TypeRole
    {
        [EnumMember]
        Primitive,

        [EnumMember]
        Enum,

        [EnumMember]
        Domain,

        [EnumMember]
        Other
    }

    public static class TypeRoleExtensions
    {
        public static bool HasSubset(this TypeRole typeRole)
        {
            switch (typeRole)
            {
                case TypeRole.Enum:
                case TypeRole.Primitive:
                    return false;

                case TypeRole.Domain:
                case TypeRole.Other:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(typeRole));
            }
        }
    }
}