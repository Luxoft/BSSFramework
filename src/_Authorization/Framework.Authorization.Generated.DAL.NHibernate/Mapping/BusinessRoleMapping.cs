using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class BusinessRoleMapping : AuthBaseMap<BusinessRole>
    {
        public BusinessRoleMapping()
        {
            this.Map(x => x.Name).Unique().Not.Nullable();
            this.Map(x => x.Description);

            this.HasMany(x => x.Permissions).AsSet().Inverse().Cascade.None();
            this.HasMany(x => x.BusinessRoleOperationLinks).AsSet().Inverse().Cascade.AllDeleteOrphan();
            this.HasMany(x => x.SubBusinessRoleLinks).AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
