using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditFlushInterceptor(IEnumerable<InlineAuditBinding> inlineAuditBindings) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is IInlineAuditDbContext { ServiceProvider: { } serviceProvider })
        {

        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = new CancellationToken()) => base.SavedChangesAsync(eventData, result, cancellationToken);

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result) => base.SavedChanges(eventData, result);
}
