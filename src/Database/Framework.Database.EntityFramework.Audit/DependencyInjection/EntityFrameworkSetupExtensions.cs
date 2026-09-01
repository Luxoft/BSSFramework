using Framework.Database.EntityFramework.DependencyInjection;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public static class EntityFrameworkSetupExtensions
{
    public static IEntityFrameworkSetup<TDbContext> AddAudit<TDbContext>(this IEntityFrameworkSetup<TDbContext> setup)
        where TDbContext : DbContext, IAuditableDbContext
    {
        return setup.AddExtension(new AuditEntityFrameworkSetupExtension());
    }
}
