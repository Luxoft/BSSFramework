using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public class AuditModelCustomizer(
    IAuditEntityFactory auditEntityFactory,
    IAuditInfoResolver auditInfoResolver,
    IAuditableEntityFilter auditableEntityFilter,
    MainAuditSchemaInfo mainSchemaInfo) : IModelCustomizer
{
    private const string RevisionColumnName = "REV";

    private const string RevisionTypeColumnName = "REVTYPE";

    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        modelBuilder.Entity<AuditRevisionEntity>(revision =>
        {
            revision.HasKey(entity => entity.Id);
            revision.ToTable(nameof(AuditRevisionEntity), mainSchemaInfo.Name);
            revision.Property(entity => entity.Author).HasColumnName(nameof(AuditRevisionEntity.Author));
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

        var revisionClrTypes = revisionTypedProjections.Select(entityType => entityType.ClrType).Append(typeof(AuditRevisionEntity)).ToHashSet();

        var auditableEntityTypes = modelBuilder
                                   .Model
                                   .GetEntityTypes()
                                   .Where(entityType => entityType.GetViewName() == null)
                                   .Where(entityType => entityType.GetTableName() != null)
                                   .Where(entityType => !entityType.IsOwned())
                                   .Where(entityType => !revisionClrTypes.Contains(entityType.ClrType))
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

            var modNameByPropertyName = entityType
                                       .GetForeignKeys()
                                       .SelectMany(fk => fk.Properties.Select(property => (property, navigationName: fk.DependentToPrincipal?.Name)))
                                       .Where(pair => pair.navigationName != null)
                                       .ToDictionary(pair => pair.property.Name, pair => pair.navigationName!);

            var discriminatorProperty = entityType.FindDiscriminatorProperty();

            var nonAuditableForeignKeyPropertyNames = entityType
                                                      .GetForeignKeys()
                                                      .Where(fk => !auditableEntityFilter.IsAuditable(fk.PrincipalEntityType))
                                                      .SelectMany(fk => fk.Properties)
                                                      .Select(property => property.Name)
                                                      .ToHashSet();

            var tableIdentifier = StoreObjectIdentifier.Table(entityType.GetTableName()!, entityType.GetSchema());

            var ownProperties = entityType.GetProperties()
                                           .Where(property => primaryKey.Properties.Contains(property) || property.GetColumnName(tableIdentifier) != null)
                                           .Concat(primaryKey.Properties)
                                           .Distinct();

            var scalarPropertyMetadata = ownProperties
                                         .Where(property => !property.IsConcurrencyToken)
                                         .Where(property => property != discriminatorProperty)
                                         .Where(property => !nonAuditableForeignKeyPropertyNames.Contains(property.Name))
                                         .Select(property => new AuditPropertyMetadata(
                                                     primaryKey.Properties.Contains(property) ? property.Name : property.GetColumnName(tableIdentifier) ?? property.Name,
                                                     property.ClrType,
                                                     primaryKey.Properties.Contains(property),
                                                     modNameByPropertyName.GetValueOrDefault(property.Name, property.Name)));

            var complexPropertyMetadata = entityType
                                          .GetComplexProperties()
                                          .Where(complexProperty => complexProperty.ComplexType.GetProperties().Any(leaf => leaf.GetColumnName(tableIdentifier) != null))
                                          .SelectMany(complexProperty => complexProperty.ComplexType
                                                                                        .GetProperties()
                                                                                        .Select(leaf => new AuditPropertyMetadata(
                                                                                                    leaf.GetColumnName(tableIdentifier) ?? leaf.Name,
                                                                                                    leaf.ClrType,
                                                                                                    false,
                                                                                                    complexProperty.Name,
                                                                                                    NestedPropertyName: leaf.Name)));

            var ownedPropertyMetadata = entityType
                                        .GetDeclaredNavigations()
                                        .Where(navigation => !navigation.IsCollection && navigation.TargetEntityType.IsOwned())
                                        .SelectMany(navigation =>
                                        {
                                            var ownedKey = navigation.TargetEntityType.FindPrimaryKey();

                                            return navigation.TargetEntityType
                                                             .GetProperties()
                                                             .Where(property => ownedKey == null || !ownedKey.Properties.Contains(property))
                                                             .Select(property => new AuditPropertyMetadata(
                                                                         property.GetColumnName(tableIdentifier) ?? property.Name,
                                                                         property.ClrType,
                                                                         false,
                                                                         navigation.Name,
                                                                         NestedPropertyName: property.Name,
                                                                         IsOwned: true));
                                        });

            var collectionModFlagMetadata = entityType
                                            .GetDeclaredNavigations()
                                            .Where(navigation => navigation.IsCollection || !navigation.IsOnDependent)
                                            .Select(navigation => navigation.Name)
                                            .Concat(entityType.GetDeclaredSkipNavigations().Select(navigation => navigation.Name))
                                            .Select(navigationName => new AuditPropertyMetadata(navigationName, typeof(bool), false, navigationName, IsModOnly: true));

            var metadata = auditEntityFactory.GetOrCreate(
                entityType.ClrType,
                scalarPropertyMetadata.Concat(complexPropertyMetadata).Concat(ownedPropertyMetadata).Concat(collectionModFlagMetadata));

            var auditEntity = modelBuilder.Entity(metadata.AuditEntityType);

            var (auditSchema, auditTableName) = auditInfoResolver.GetInfo(entityType);

            auditEntity.ToTable(auditTableName, auditSchema);
            auditEntity.HasKey(keyPropertyNames.Append(auditEntityFactory.RevisionIdPropertyName).ToArray());
            auditEntity.Property(auditEntityFactory.RevisionIdPropertyName).HasColumnName(RevisionColumnName);

            if (entityType.BaseType == null)
            {
                auditEntity.Property(auditEntityFactory.RevisionTypePropertyName).HasColumnName(RevisionTypeColumnName).HasColumnType("smallint");
            }
            else
            {
                auditEntity.Ignore(auditEntityFactory.RevisionTypePropertyName);
            }

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
