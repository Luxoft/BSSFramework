using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Audit;

public class AuditModelCustomizer(
    IAuditEntityFactory auditEntityFactory,
    ModelCustomizerDependencies dependencies,
    AuditInfo auditInfo) : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        ArgumentException.ThrowIfNullOrWhiteSpace(auditInfo.SchemaName);

        modelBuilder.Entity<AuditRevisionEntity>(revision =>
        {
            revision.HasKey(entity => entity.Id);
            revision.ToTable(nameof(AuditRevisionEntity), auditInfo.SchemaName);
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(entityType => entityType.ClrType != typeof(AuditRevisionEntity))
                     .ToArray())
        {
            var primaryKey = entityType.FindPrimaryKey()!;
            var metadata = auditEntityFactory.GetOrCreate(
                entityType.ClrType,
                entityType.GetProperties()
                    .Select(property => new AuditPropertyMetadata(
                        property.Name,
                        property.ClrType,
                        primaryKey.Properties.Contains(property))));
            var auditEntity = modelBuilder.Entity(metadata.AuditEntityType);

            var sourceSchema = entityType.GetSchema();
            var auditTableName = string.IsNullOrEmpty(sourceSchema)
                ? $"{entityType.ClrType.Name}Audits"
                : $"{sourceSchema}_{entityType.ClrType.Name}Audits";
            auditEntity.ToTable(auditTableName, auditInfo.SchemaName);
            auditEntity.HasKey(
                primaryKey.Properties.Select(property => property.Name)
                    .Append(auditEntityFactory.RevisionIdPropertyName)
                    .ToArray());
            auditEntity
                .HasOne(typeof(AuditRevisionEntity), auditEntityFactory.RevisionPropertyName)
                .WithMany()
                .HasForeignKey(auditEntityFactory.RevisionIdPropertyName);
        }
    }
}
