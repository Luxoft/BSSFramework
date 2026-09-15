using Framework.Configuration.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

public abstract class ConfigurationBaseMap<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditPersistentDomainObjectBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name, "configuration");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.CreatedBy).HasMaxLength(255);
        builder.Property(x => x.CreateDate);
        builder.Property(x => x.ModifiedBy).HasMaxLength(255);
        builder.Property(x => x.ModifyDate);
    }
}
