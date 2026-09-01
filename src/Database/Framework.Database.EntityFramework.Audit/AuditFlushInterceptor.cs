using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Audit;

public class AuditFlushInterceptor() : SaveChangesInterceptor
{
    private readonly ConditionalWeakTable<DbContext, List<AuditEntry>> pendingAudits = [];

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        this.CaptureChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        this.CaptureChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        this.WriteAudits(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await this.WriteAuditsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void CaptureChanges(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var auditEntityFactory = dbContext.GetService<IAuditEntityFactory>();
        var audits = dbContext.ChangeTracker.Entries()
                              .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                              .Select(entry => this.CreateAuditEntry(entry, auditEntityFactory))
                              .OfType<AuditEntry>()
                              .ToList();

        if (audits.Count > 0)
        {
            this.pendingAudits.Remove(dbContext);
            this.pendingAudits.Add(dbContext, audits);
        }
    }

    private void WriteAudits(DbContext? dbContext)
    {
        if (dbContext is null
            || !this.pendingAudits.TryGetValue(dbContext, out var audits)
            || dbContext is not IAuditableDbContext auditableDbContext)
        {
            return;
        }

        this.pendingAudits.Remove(dbContext);
        var revision = this.AddAuditEntities(dbContext, auditableDbContext, audits, dbContext.GetService<IAuditEntityFactory>());
        dbContext.SaveChanges();

        auditableDbContext.CurrentRevisionState.CurrentRevision = revision.Id;
    }

    private async Task WriteAuditsAsync(DbContext? dbContext, CancellationToken cancellationToken)
    {
        if (dbContext is null
            || !this.pendingAudits.TryGetValue(dbContext, out var audits)
            || dbContext is not IAuditableDbContext auditableDbContext)
        {
            return;
        }

        this.pendingAudits.Remove(dbContext);
        var revision = this.AddAuditEntities(dbContext, auditableDbContext, audits, dbContext.GetService<IAuditEntityFactory>());
        await dbContext.SaveChangesAsync(cancellationToken);

        auditableDbContext.CurrentRevisionState.CurrentRevision = revision.Id;
    }

    private AuditEntry? CreateAuditEntry(EntityEntry entry, IAuditEntityFactory auditEntityFactory)
    {
        if (!auditEntityFactory.TryGet(entry.Metadata.ClrType, out var metadata))
        {
            return null;
        }

        var modifiedProperties = metadata.Properties
                                         .Where(property => !property.IsKey)
                                         .ToDictionary(
                                             property => property.Name,
                                             property => this.IsPropertyModified(entry, property));

        return new AuditEntry(entry, metadata, this.ToRevisionType(entry.State), modifiedProperties);
    }

    private AuditRevisionEntity AddAuditEntities(
        DbContext dbContext,
        IAuditableDbContext auditableDbContext,
        List<AuditEntry> audits,
        IAuditEntityFactory auditEntityFactory)
    {
        var revision = new AuditRevisionEntity
                       {
                           RevisionDate = auditableDbContext.TimeProvider.GetUtcNow().DateTime, Author = auditableDbContext.CurrentUser.Name
                       };
        dbContext.Set<AuditRevisionEntity>().Add(revision);

        foreach (var audit in audits)
        {
            var auditEntity = Activator.CreateInstance(audit.Metadata.AuditEntityType)!;
            foreach (var property in audit.Metadata.Properties)
            {
                if (!property.IsModOnly)
                {
                    var value = this.GetCurrentValue(audit.Entry, property);
                    audit.Metadata.AuditEntityType.GetProperty(property.Name)!.SetValue(auditEntity, value);
                }

                if (!property.IsKey)
                {
                    audit.Metadata.AuditEntityType.GetProperty($"{property.ModName}_MOD")!
                         .SetValue(auditEntity, audit.ModifiedProperties[property.Name]);
                }
            }

            audit.Metadata.AuditEntityType
                 .GetProperty(auditEntityFactory.RevisionPropertyName)!
                 .SetValue(auditEntity, revision);
            audit.Metadata.AuditEntityType
                 .GetProperty(auditEntityFactory.RevisionTypePropertyName)!
                 .SetValue(auditEntity, audit.RevisionType);
            dbContext.Add(auditEntity);
        }

        return revision;
    }

    private AuditRevisionType ToRevisionType(EntityState state) => state switch
    {
        EntityState.Added => AuditRevisionType.Added,
        EntityState.Modified => AuditRevisionType.Modified,
        EntityState.Deleted => AuditRevisionType.Deleted,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };

    private bool IsPropertyModified(EntityEntry entry, AuditPropertyMetadata property) =>
        entry.State == EntityState.Added
        || entry.State == EntityState.Modified && !property.IsModOnly && this.GetPropertyEntry(entry, property) is { IsModified: true };

    private object? GetCurrentValue(EntityEntry entry, AuditPropertyMetadata property) =>
        this.GetPropertyEntry(entry, property)?.CurrentValue;

    private PropertyEntry? GetPropertyEntry(EntityEntry entry, AuditPropertyMetadata property)
    {
        if (property.NestedPropertyName is null)
        {
            return entry.Property(property.Name);
        }

        if (property.IsOwned)
        {
            var targetEntry = entry.Reference(property.ModName).TargetEntry;
            return targetEntry?.Property(property.NestedPropertyName);
        }

        return entry.ComplexProperty(property.ModName).Property(property.NestedPropertyName);
    }

    private sealed record AuditEntry(
        EntityEntry Entry,
        AuditEntityMetadata Metadata,
        AuditRevisionType RevisionType,
        IReadOnlyDictionary<string, bool> ModifiedProperties);
}
