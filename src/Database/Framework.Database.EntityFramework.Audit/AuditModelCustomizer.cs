using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Audit;

public class AuditModelCustomizer(
    IAuditEntityFactory auditEntityFactory,
    ModelCustomizerDependencies dependencies,
    IAuditInfoResolver auditInfoResolver,
    MainSchemaInfo mainSchemaInfo) : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        modelBuilder.Entity<AuditRevisionEntity>(revision =>
        {
            revision.HasKey(entity => entity.Id);
            revision.ToTable(nameof(AuditRevisionEntity), mainSchemaInfo.Name);
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

            var (auditSchema, auditTableName) = auditInfoResolver.GetInfo(entityType);

            auditEntity.ToTable(auditTableName, auditSchema);
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
