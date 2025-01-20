using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping;

public class PermissionRestrictionMap : AuthBaseMap<PermissionRestriction>
{
    public PermissionRestrictionMap()
    {
        this.Map(x => x.SecurityContextId).Not.Nullable()
            .UniqueKey("UIX_permission_securityContextId_securityContextTypePermissionRestriction");
        this.References(x => x.Permission).Column($"{nameof(PermissionRestriction.Permission)}Id").Not.Nullable()
            .UniqueKey("UIX_permission_securityContextId_securityContextTypePermissionRestriction");
        this.References(x => x.SecurityContextType).Column($"{nameof(PermissionRestriction.SecurityContextType)}Id").Not.Nullable()
            .UniqueKey("UIX_permission_securityContextId_securityContextTypePermissionRestriction");
    }
}
