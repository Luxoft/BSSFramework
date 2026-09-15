using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditFlushInterceptor(IAuditPropertiesSetterMapFactory auditPropertiesSetterMapFactory) : SaveChangesInterceptor
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
        CancellationToken cancellationToken = new CancellationToken())
    {
        this.SetAuditFields(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void SetAuditFields(DbContext? context)
    {
        if (context is IInlineAuditDbContext { ServiceProvider: { } serviceProvider })
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                var actions = entry.State switch
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
