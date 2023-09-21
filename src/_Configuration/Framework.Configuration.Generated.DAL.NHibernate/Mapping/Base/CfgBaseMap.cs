using FluentNHibernate.Mapping;

using Framework.Configuration.Domain;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base
{
    public abstract class CfgBaseMap<TEntity> : ClassMap<TEntity>
        where TEntity : AuditPersistentDomainObjectBase
    {
        protected CfgBaseMap()
        {
            this.Schema("configuration");

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
