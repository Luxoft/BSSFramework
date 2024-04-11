using System.Runtime.Serialization;

namespace Framework.Authorization.Generated.DTO;

public partial class PermissionRestrictionStrictDTO
{
    [DataMember]
    public SecurityEntityIdentityDTO SecurityEntity
    {
        get;
        set;
    }

    [DataMember]
    public EntityTypeIdentityDTO EntityType
    {
        get;
        set;
    }
}
