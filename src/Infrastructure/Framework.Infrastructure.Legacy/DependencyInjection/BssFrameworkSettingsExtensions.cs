using Framework.Application.Events;
using Framework.BLL.DependencyInjection;
using Framework.BLL.DTOMapping.DTOMapper;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class BssFrameworkSettingsExtensions
{
    extension(IBssFrameworkSetup builder)
    {
        public IBssFrameworkSetup AddSubscriptionManager<TSubscriptionManager>()
            where TSubscriptionManager : class, IEventOperationReceiver =>
            builder.AddServices(services => services.AddSubscriptionManagers(setup => setup.Add<TSubscriptionManager>()));

        public IBssFrameworkSetup AddLegacyGenericServices() => builder.AddServices(services => services.AddLegacyGenericServices());

        public IBssFrameworkSetup AddContextEvaluators() => builder.AddServices(services => services.AddContextEvaluators());

        public IBssFrameworkSetup AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(Action<BLLSystemSettings>? setupAction = null)
            where TBLLContextImpl : TBLLContextDecl =>
            builder.AddServices(services => services.AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(setupAction));

        public IBssFrameworkSetup SetDTOMapping<TDTOMappingService, TDTOMappingServiceImpl, TPersistentDomainObjectBase, TEventDTOBase>()
            where TDTOMappingService : class
            where TDTOMappingServiceImpl : class, TDTOMappingService =>
            builder.AddServices(services => services.AddScoped<TDTOMappingService, TDTOMappingServiceImpl>()
                                                    .AddScoped<IDomainEventDTOMapper<TPersistentDomainObjectBase>, RuntimeDomainEventDTOMapper<TPersistentDomainObjectBase,
                                                        TDTOMappingService, TEventDTOBase>>());
    }
}
