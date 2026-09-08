using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditFlushInterceptor(IEnumerable<InlineAuditBinding> inlineAuditBindings) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        //var

        return base.SavingChanges(eventData, result);
    }
}
