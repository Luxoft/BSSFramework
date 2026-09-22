using System.Data.Common;

using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditFlushInterceptor(IAuditPropertiesSetterMapFactory auditPropertiesSetterMapFactory) : SaveChangesInterceptor, IDbConnectionInterceptor
{
    private static readonly InlineAuditAction[] AddedActions = [InlineAuditAction.Create, InlineAuditAction.Modify];

    private static readonly InlineAuditAction[] ModifiedActions = [InlineAuditAction.Modify];

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        this.SetAuditFields(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct)
    {
        this.SetAuditFields(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    // NHibernate assigns inline-audit "Create" values synchronously when an entity is saved into the session
    // (before flush), so a domain object reads its own audit values right after SaveAsync. EF only invokes
    // SavingChanges at flush time (which may happen much later, e.g. on session close), so we mirror the
    // NHibernate timing here by reacting to ChangeTracker.Tracked, which fires as soon as an entity is added.
    public void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
        => this.SubscribeTracked(eventData.Context);

    public Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken ct)
    {
        this.SubscribeTracked(eventData.Context);

        return Task.CompletedTask;
    }

    private void SubscribeTracked(DbContext? context)
    {
        if (context != null)
        {
            context.ChangeTracker.Tracked -= this.OnTracked;
            context.ChangeTracker.Tracked += this.OnTracked;
        }
    }

    private void OnTracked(object? sender, EntityTrackedEventArgs e)
    {
        if (!e.FromQuery && e.Entry.State == EntityState.Added)
        {
            this.SetAuditFields(e.Entry.Context, [e.Entry], AddedActions);
        }
    }

    private void SetAuditFields(DbContext? context) =>
        this.SetAuditFields(
            context,
            context?.ChangeTracker.Entries() ?? [],
            null);

    private void SetAuditFields(DbContext? context, IEnumerable<EntityEntry> entries, InlineAuditAction[]? actionsOverride)
    {
        if (context is IInlineAuditDbContext { ServiceProvider: { } serviceProvider })
        {
            foreach (var entry in entries)
            {
                var actions = actionsOverride ?? entry.State switch
                {
                    EntityState.Added => AddedActions,
                    EntityState.Modified => ModifiedActions,
                    _ => []
                };

                if (actions.Length == 0)
                {
                    continue;
                }

                actions.Select(action => auditPropertiesSetterMapFactory.CreateSetterMap(entry.Entity.GetType(), action))
                       .Aggregate()
                       .CreateSetter(serviceProvider)
                       .SetAuditFields(entry);
            }
        }
    }
}
