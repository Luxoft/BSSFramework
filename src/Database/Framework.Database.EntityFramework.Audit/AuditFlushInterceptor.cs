using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Framework.Database.EntityFramework.Sessions;

namespace Framework.Database.EntityFramework.Audit;

public class AuditFlushInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
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

    private void CaptureChanges(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var auditEntityFactory = context.GetService<IAuditEntityFactory>();
        var audits = context.ChangeTracker.Entries()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Select(entry => this.CreateAuditEntry(entry, auditEntityFactory))
            .OfType<AuditEntry>()
            .ToList();

        if (audits.Count > 0)
        {
            this.pendingAudits.Remove(context);
            this.pendingAudits.Add(context, audits);
        }
    }

    private void WriteAudits(DbContext? context)
    {
        if (context is null || !this.pendingAudits.TryGetValue(context, out var audits))
        {
            return;
        }

        this.pendingAudits.Remove(context);
        var revision = this.AddAuditEntities(context, audits, context.GetService<IAuditEntityFactory>());
        context.SaveChanges();
        EfCurrentRevisionStore.SetCurrentRevision(context, revision.Id);
    }

    private async Task WriteAuditsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null || !this.pendingAudits.TryGetValue(context, out var audits))
        {
            return;
        }

        this.pendingAudits.Remove(context);
        var revision = this.AddAuditEntities(context, audits, context.GetService<IAuditEntityFactory>());
        await context.SaveChangesAsync(cancellationToken);
        EfCurrentRevisionStore.SetCurrentRevision(context, revision.Id);
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
        DbContext context,
        List<AuditEntry> audits,
        IAuditEntityFactory auditEntityFactory)
    {
        var revision = new AuditRevisionEntity { RevisionDate = timeProvider.GetUtcNow().DateTime };
        context.Set<AuditRevisionEntity>().Add(revision);

        foreach (var audit in audits)
        {
            var auditEntity = Activator.CreateInstance(audit.Metadata.AuditEntityType)!;
            foreach (var property in audit.Metadata.Properties)
            {
                if (!property.IsModOnly)
                {
                    var value = audit.Entry.Property(property.Name).CurrentValue;
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
            context.Add(auditEntity);
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
        entry.State == EntityState.Added ||
        entry.State == EntityState.Modified && !property.IsModOnly && entry.Property(property.Name).IsModified;

    private sealed record AuditEntry(
        EntityEntry Entry,
        AuditEntityMetadata Metadata,
        AuditRevisionType RevisionType,
        IReadOnlyDictionary<string, bool> ModifiedProperties);
}
