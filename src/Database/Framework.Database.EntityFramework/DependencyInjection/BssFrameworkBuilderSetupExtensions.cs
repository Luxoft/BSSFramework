using Framework.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.DependencyInjection;

public static class BssFrameworkBuilderSetupExtensions
{
    extension<TSelf>(IBssFrameworkSetup<TSelf> setup)
        where TSelf : IBssFrameworkSetup<TSelf>
    {
        public TSelf AddEntityFramework<TDbContext>(Action<IEntityFrameworkSetup<TDbContext>>? setupAction = null)
            where TDbContext : DbContext =>

            setup.AddExtensions(new BssFrameworkExtension(services => services.AddEntityFramework(setupAction)));
    }
}
