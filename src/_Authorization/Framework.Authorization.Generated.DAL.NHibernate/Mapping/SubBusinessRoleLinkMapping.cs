using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class SubBusinessRoleLinkMapping : AuthBaseMap<SubBusinessRoleLink>
    {
        public SubBusinessRoleLinkMapping()
        {
            this.References(x => x.BusinessRole).Column($"{nameof(SubBusinessRoleLink.BusinessRole)}Id")
                .Not.Nullable()
                .UniqueKey("UIX_businessRole_subBusinessRoleSubBusinessRoleLink");
            this.References(x => x.SubBusinessRole).Column($"{nameof(SubBusinessRoleLink.SubBusinessRole)}Id")
                .Not.Nullable()
                .UniqueKey("UIX_businessRole_subBusinessRoleSubBusinessRoleLink");
        }
    }
}
