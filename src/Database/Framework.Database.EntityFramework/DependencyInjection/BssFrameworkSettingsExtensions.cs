using Framework.Infrastructure.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public static class BssFrameworkBuilderSetupExtensions
{
    public static TSelf AddEntityFramework<TSelf>(this IBssFrameworkSetup<TSelf> setup, Action<IEntityFrameworkSetup>? setupAction = null) =>

        setup.AddExtensions(new BssFrameworkExtension(services => services.AddEntityFramework(setupAction)));
}
