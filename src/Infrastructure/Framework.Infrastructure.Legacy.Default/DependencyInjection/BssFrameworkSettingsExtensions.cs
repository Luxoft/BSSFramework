using Anch.DependencyInjection;

using Framework.Configuration.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkSetup builder)
    {
        public IBssFrameworkSetup AddSystemConstant(Type systemConstantContainerType) =>
            builder.AddServices(services => services.AddSingleton(new SystemConstantInfo(systemConstantContainerType)));

        public IBssFrameworkSetup AddLegacyDefaultGenericServices() => builder.AddServices(services => services.AddLegacyDefaultGenericServices());

        public IBssFrameworkSetup AddConfigurationTargetSystems(Action<ITargetSystemSetup> setupAction) =>

            builder.AddServices(services => services.Initialize<TargetSystemSetup>(setupAction));
    }
}
