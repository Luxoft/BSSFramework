using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping;

public class PrincipalMap : AuthBaseMap<Principal>
{
    public PrincipalMap()
    {
        this.Map(x => x.Name).Not.Nullable().UniqueKey("UIX_namePrincipal");
        this.References(x => x.RunAs).Column($"{nameof(Principal.RunAs)}Id");
        this.HasMany(x => x.Permissions).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}
