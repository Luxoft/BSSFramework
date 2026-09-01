using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.DependencyInjection;

public interface IEntityFrameworkSetup<TDbContext>
    where TDbContext : DbContext
{
    //bool AddDefaultListener { get; set; }

    //bool AddDefaultInitializer { get; set; }

    //bool AutoAddFluentMapping { get; set; }

    IEntityFrameworkSetup<TDbContext> AddExtension(IEntityFrameworkSetupExtension extension);
}
