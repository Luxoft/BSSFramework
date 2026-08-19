using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

public abstract class SampleSystemBaseMap<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditPersistentDomainObjectBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name, "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Version).ValueGeneratedNever().IsConcurrencyToken().IsRequired();
        builder.Property(x => x.Active).IsRequired();
        builder.Property(x => x.CreateDate);
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.ModifiedBy);
        builder.Property(x => x.ModifyDate);
    }
}
