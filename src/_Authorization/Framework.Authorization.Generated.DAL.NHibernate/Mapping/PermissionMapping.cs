using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class PermissionMapping : AuthBaseMap<Permission>
    {
        public PermissionMapping()
        {
            this.Map(x => x.Comment);
            this.Map(x => x.IsDelegatedFrom);
            this.Map(x => x.IsDelegatedTo);
            this.Map(x => x.Status).Not.Nullable();
            this.References(x => x.DelegatedFrom).Column($"{nameof(Permission.DelegatedFrom)}Id");
            this.References(x => x.Principal).Column($"{nameof(Permission.Principal)}Id").Not.Nullable();
            this.References(x => x.Role).Column($"{nameof(Permission.Role)}Id").Not.Nullable();
            this.Component(
                x => x.Period,
                part =>
                {
                    part.Map(x => x.EndDate).Column("periodendDate");
                    part.Map(x => x.StartDate).Column("periodstartDate");
                });

            this.HasMany(x => x.DelegatedTo).AsSet().Inverse().Cascade.AllDeleteOrphan();
            this.HasMany(x => x.FilterItems).AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
