using FluentNHibernate.Mapping;

using Framework.Configuration.Domain;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

public abstract class ConfigurationBaseMap<TEntity> : ClassMap<TEntity>
    where TEntity : AuditPersistentDomainObjectBase
{
    protected ConfigurationBaseMap()
    {
        this.Schema("configuration");

        this.DynamicUpdate();

        this.Id(x => x.Id).GeneratedBy.GuidComb();

        this.Map(x => x.Active);
        this.Map(x => x.CreatedBy);
        this.Map(x => x.CreateDate);
        this.Map(x => x.ModifiedBy);
        this.Map(x => x.ModifyDate);
    }
}
