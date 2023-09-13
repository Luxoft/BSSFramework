using FluentNHibernate.Mapping;

using Framework.Authorization.Domain;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base
{
    public abstract class AuthBaseMap<TEntity> : ClassMap<TEntity>
        where TEntity : AuditPersistentDomainObjectBase
    {
        protected AuthBaseMap()
        {
            this.Schema("auth");

            this.DynamicUpdate();

            this.Id(x => x.Id).GeneratedBy.GuidComb();

            this.Map(x => x.Active).Not.Nullable();
            this.Map(x => x.CreatedBy).Not.Nullable().Length(int.MaxValue);
            this.Map(x => x.CreateDate).Not.Nullable();
            this.Map(x => x.ModifiedBy).Not.Nullable().Length(int.MaxValue);
            this.Map(x => x.ModifyDate).Not.Nullable();
        }
    }
}
