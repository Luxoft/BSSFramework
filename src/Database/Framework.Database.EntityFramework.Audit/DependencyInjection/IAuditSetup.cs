using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public interface IAuditSetup
{
    IAuditSetup SetFilter(Func<IReadOnlyEntityType, bool> isAuditable);
}
