using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class PrincipalMapping : AuthBaseMap<Principal>
    {
        public PrincipalMapping()
        {
            this.Map(x => x.ExternalId);
            this.Map(x => x.Name).Not.Nullable();
            this.References(x => x.RunAs).Column($"{nameof(Principal.RunAs)}Id");

            this.HasMany(x => x.Permissions).AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
