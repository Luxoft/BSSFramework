using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.DependencyInjection;

public interface IEntityFrameworkSetup
{
    //bool AddDefaultListener { get; set; }

    //bool AddDefaultInitializer { get; set; }

    //bool AutoAddFluentMapping { get; set; }

    IEntityFrameworkSetup SetDbContext<TDbContext>()
        where TDbContext : DbContext;

    IEntityFrameworkSetup AddExtension(IEntityFrameworkSetupExtension extension);
}
