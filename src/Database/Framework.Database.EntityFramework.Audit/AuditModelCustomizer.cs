using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public class AuditModelCustomizer(
    IAuditEntityFactory auditEntityFactory,
    ModelCustomizerDependencies dependencies,
    IAuditInfoResolver auditInfoResolver,
    MainSchemaInfo mainSchemaInfo) : ModelCustomizer(dependencies)
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
        });

        var allEntityTypes = modelBuilder.Model.GetEntityTypes().ToArray();

        var typedProjectionsByTable = allEntityTypes
            .GroupBy(entityType => (entityType.GetSchema(), entityType.GetTableName()))
            .ToDictionary(group => group.Key, group => group.First());

        // Entities already living in an "...Audit" schema are either the generic revision table or a typed
        // projection of another entity's audit table (see below) - they must not be audited themselves.
        bool IsAuditSchemaEntity(IReadOnlyEntityType entityType) =>
            entityType.GetSchema() is { } schema && schema.EndsWith("Audit", StringComparison.Ordinal);

        var auditableEntityTypes = allEntityTypes.Where(
            entityType => entityType.ClrType != typeof(AuditRevisionEntity) && !IsAuditSchemaEntity(entityType));

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

            // Some entities (e.g. SampleSystem.Domain.BU.BusinessUnit) already have a hand-written, strongly typed
            // projection of their audit table (e.g. SampleSystem.AuditDomain.BusinessUnitAudit), mirroring the
            // legacy NHibernate mapping in Audit.hbm.xml. It shares the same table as the dynamically generated
            // audit entity above (table splitting), so EF requires a linking relationship between the two. The
            // dynamic entity's columns are the source of truth - the typed projection's own map is responsible
            // for pointing at whatever column names the dynamic entity ends up with.
            if (typedProjectionsByTable.TryGetValue((auditSchema, auditTableName), out var typedProjection)
                && typedProjection.ClrType != metadata.AuditEntityType)
            {
                modelBuilder.Entity(typedProjection.ClrType)
                            .HasOne(metadata.AuditEntityType)
                            .WithOne()
                            .HasForeignKey(typedProjection.ClrType, keyPropertyNames.Append(RevisionColumnName).ToArray());
            }
        }
    }
}
