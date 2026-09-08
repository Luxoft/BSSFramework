using Framework.Database.EntityFramework.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public static class EntityFrameworkSetupExtensions
{
    public static IEntityFrameworkSetup<TDbContext> AddEnversAudit<TDbContext>(this IEntityFrameworkSetup<TDbContext> setup)
        where TDbContext : DbContext, IEnversAuditDbContext =>
        setup.AddExtension(new EnversAuditEntityFrameworkSetupExtension());
}
