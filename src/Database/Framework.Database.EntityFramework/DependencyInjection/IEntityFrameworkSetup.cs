using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.DependencyInjection;

public interface IEntityFrameworkSetup<TDbContext>
    where TDbContext : DbContext
{
    IEntityFrameworkSetup<TDbContext> AddExtension(IEntityFrameworkSetupExtension extension);
}
