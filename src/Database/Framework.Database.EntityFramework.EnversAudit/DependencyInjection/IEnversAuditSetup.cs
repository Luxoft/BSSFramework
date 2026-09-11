using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public interface IEnversAuditSetup
{
    IEnversAuditSetup SetFilter(Func<IReadOnlyEntityType, bool> isAuditable);
}
