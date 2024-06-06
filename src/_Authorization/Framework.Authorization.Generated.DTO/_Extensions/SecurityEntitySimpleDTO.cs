using System.Runtime.Serialization;

using Framework.Core;
using Framework.Persistent;

namespace Framework.Authorization.Generated.DTO;

public partial class SecurityEntitySimpleDTO : IIdentityObjectContainer<SecurityEntityIdentityDTO>, IEquatable<SecurityEntitySimpleDTO>
{
    [IgnoreDataMember]
    public SecurityEntityIdentityDTO Identity => new SecurityEntityIdentityDTO(this.Id);

    public override bool Equals(object obj)
    {
        return this.Equals(obj as SecurityEntitySimpleDTO);
    }

    public bool Equals(SecurityEntitySimpleDTO other)
    {
        return !object.ReferenceEquals(other, null) && !this.Id.IsDefault() && this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return this.Id.IsDefault() ? base.GetHashCode() : this.Id.GetHashCode();
    }
}
