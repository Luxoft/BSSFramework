using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Audit;

public class AuditModelCustomizer(
    IAuditEntityFactory auditEntityFactory,
    ModelCustomizerDependencies dependencies,
    IAuditInfoResolver auditInfoResolver,
    IAuditableEntityFilter auditableEntityFilter,
    MainAuditSchemaInfo mainSchemaInfo) : ModelCustomizer(dependencies)
{
    private const string RevisionColumnName = "REV";

    private const string RevisionTypeColumnName = "REVTYPE";

    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        modelBuilder.Entity<AuditRevisionEntity>(revision =>
        {
            revision.HasKey(entity => entity.Id);
            revision.ToTable(nameof(AuditRevisionEntity), mainSchemaInfo.Name);
            revision.Property(entity => entity.RevisionDate).HasColumnName(nameof(AuditRevisionEntity.RevisionDate));
        });

        var revisionTypedProjections = modelBuilder
                                       .Model
                                       .GetEntityTypes()
                                       .Where(entityType => entityType.ClrType != typeof(AuditRevisionEntity))
                                       .Where(entityType => string.Equals(entityType.GetTableName(), nameof(AuditRevisionEntity), StringComparison.OrdinalIgnoreCase))
                                       .Where(entityType => string.Equals(entityType.GetSchema(), mainSchemaInfo.Name, StringComparison.OrdinalIgnoreCase))
                                       .ToArray();

        foreach (var typedProjection in revisionTypedProjections)
        {
            modelBuilder.Entity(typedProjection.ClrType).ToTable(nameof(AuditRevisionEntity), mainSchemaInfo.Name);
        }

        var auditableEntityTypes = modelBuilder
                                   .Model
                                   .GetEntityTypes()
                                   .Where(entityType => entityType.GetViewName() == null)
                                   .Where(auditableEntityFilter.IsAuditable)
                                   .ToArray();

        var typedProjectionsByTable = modelBuilder
                                      .Model
                                      .GetEntityTypes()
                                      .GroupBy(entityType => (entityType.GetSchema(), entityType.GetTableName()))
                                      .ToDictionary(g => g.Key, g => g.ToArray());

        foreach (var entityType in auditableEntityTypes)
        {
            var primaryKey = entityType.FindPrimaryKey()!;
            var keyPropertyNames = primaryKey.Properties.Select(property => property.Name).ToArray();

            var metadata = auditEntityFactory.GetOrCreate(
                entityType.ClrType,
                entityType.GetProperties()
                          .Select(property => new AuditPropertyMetadata(
                                      property.Name,
                                      property.ClrType,
                                      primaryKey.Properties.Contains(property))));

            var auditEntity = modelBuilder.Entity(metadata.AuditEntityType);

            var (auditSchema, auditTableName) = auditInfoResolver.GetInfo(entityType);

            auditEntity.ToTable(auditTableName, auditSchema);
            auditEntity.HasKey(keyPropertyNames.Append(auditEntityFactory.RevisionIdPropertyName).ToArray());
            auditEntity.Property(auditEntityFactory.RevisionIdPropertyName).HasColumnName(RevisionColumnName);
            auditEntity.Property(auditEntityFactory.RevisionTypePropertyName).HasColumnName(RevisionTypeColumnName);
            auditEntity
                .HasOne(typeof(AuditRevisionEntity), auditEntityFactory.RevisionPropertyName)
                .WithMany()
                .HasForeignKey(auditEntityFactory.RevisionIdPropertyName);

            if (typedProjectionsByTable.TryGetValue((auditSchema, auditTableName), out var typedProjections))
            {
                foreach (var typedProjection in typedProjections.Where(typedProjection => typedProjection.ClrType != metadata.AuditEntityType))
                {
                    modelBuilder.Entity(typedProjection.ClrType)
                                .HasOne(metadata.AuditEntityType)
                                .WithOne()
                                .HasForeignKey(typedProjection.ClrType, keyPropertyNames.Append(RevisionColumnName).ToArray());
                }
            }
        }
    }
}
