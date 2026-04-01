using Framework.Application.Events;
using Framework.BLL.DependencyInjection;
using Framework.BLL.DTOMapping.DTOMapper;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkBuilder builder)
    {
        public IBssFrameworkBuilder AddSubscriptionManager<TSubscriptionManager>()
            where TSubscriptionManager : class, IEventOperationReceiver
        {
            return builder.AddServices(services => services.AddSubscriptionManagers(setup => setup.Add<TSubscriptionManager>()));
        }

        public IBssFrameworkBuilder AddLegacyGenericServices()
        {
            return builder.AddServices(services => services.AddLegacyGenericServices());
        }

        public IBssFrameworkBuilder AddContextEvaluators()
        {
            return builder.AddServices(services => services.AddContextEvaluators());
        }

        public IBssFrameworkBuilder AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(Action<BLLSystemSettings>? setupAction = null)
            where TBLLContextImpl : TBLLContextDecl
        {
            return builder.AddServices(services => services.AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(setupAction));
        }

        public IBssFrameworkBuilder SetDTOMapping<TDTOMappingService, TDTOMappingServiceImpl, TPersistentDomainObjectBase, TEventDTOBase>()
            where TDTOMappingService : class
            where TDTOMappingServiceImpl : class, TDTOMappingService =>
            builder.AddServices(services => services.AddScoped<TDTOMappingService, TDTOMappingServiceImpl>()
                                                    .AddScoped<IDomainEventDTOMapper<TPersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase,
                                                        TDTOMappingService, TEventDTOBase>>());
    }
}
