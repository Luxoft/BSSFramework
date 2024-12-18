using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping;

public class BusinessRoleMap : AuthBaseMap<BusinessRole>
{
    public BusinessRoleMap()
    {
        this.Map(x => x.Name).Unique().Not.Nullable();
        this.Map(x => x.Description);
        this.HasMany(x => x.Permissions).AsSet().Inverse().Cascade.None();
    }
}
