using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping;

public class SecurityContextTypeMap : AuthBaseMap<SecurityContextType>
{
    public SecurityContextTypeMap() => this.Map(x => x.Name).Not.Nullable();
}
