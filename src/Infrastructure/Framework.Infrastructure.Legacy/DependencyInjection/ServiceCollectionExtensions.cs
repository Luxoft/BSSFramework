using System.Reflection;

using CommonFramework.DependencyInjection;

using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Services;
using Framework.BLL.Validation;
using Framework.Core;
using Framework.Infrastructure.ApiControllerBaseEvaluator;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Infrastructure.Service;
using Framework.Projection;
using Framework.Tracking;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.DomainServices;
using SecuritySystem.DomainServices.DependencySecurity;
using SecuritySystem.SecurityRuleInfo;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLegacyGenericServices()
        {
            services.AddScoped(typeof(EvaluatedData<,>));
            services.AddKeyedScoped<IEventOperationSender, BLLEventOperationSender>("BLL");

            services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());
            services.AddScoped(typeof(ITrackingService<>), typeof(TrackingService<>));

            services.ReplaceSingleton<IActualDomainTypeResolver, ProjectionActualDomainTypeResolver>();
            services.ReplaceSingleton<ISecurityContextInfoSource, ProjectionSecurityContextInfoSource>();

            services.AddSingleton<IExceptionExpander, TargetInvocationExceptionExpander>();

            return services;
        }

        public IServiceCollection AddSubscriptionManagers(Action<ISubscriptionManagerSetupObject> setup)
        {
            var setupObject = new SubscriptionManagerSetupObject();

            setup(setupObject);

            foreach (var setupObjectInitAction in setupObject.InitActions)
            {
                setupObjectInitAction(services);
            }

            return services;
        }

        public IServiceCollection AddContextEvaluators(bool useSingleCall = false)
        {
            services.AddScoped(
                typeof(IApiControllerBaseEvaluator<,>),
                useSingleCall ? typeof(ApiControllerBaseSingleCallEvaluator<,>) : typeof(ApiControllerNewScopeEvaluator<,>));

            if (!useSingleCall)
            {
                services.AddSingleton(typeof(IContextEvaluator<,>), typeof(ContextEvaluator<,>));
            }

            return services;
        }

        public IServiceCollection AddProjectionDomainSecurityServices(Assembly assembly)
        {
            var projectionsRequest = from type in assembly.GetTypes()

                                     let projectionAttr = type.GetCustomAttribute<ProjectionAttribute>()

                                     where projectionAttr != null && type.HasAttribute<DependencySecurityAttribute>()

                                     select new { DomainType = type, projectionAttr.SourceType, CustomViewSecurityRule = (DomainSecurityRule?)type.GetViewSecurityRule() };

            foreach (var pair in projectionsRequest)
            {
                services.AddScoped(
                    typeof(IDomainSecurityService<>).MakeGenericType(pair.DomainType),
                    typeof(UntypedDependencyDomainSecurityService<,>).MakeGenericType(pair.DomainType, pair.SourceType));

                if (pair.CustomViewSecurityRule != null)
                {
                    services.AddSingleton(
                        new DomainModeSecurityRuleInfo(SecurityRule.View.ToDomain(pair.DomainType), pair.CustomViewSecurityRule));
                }
            }

            return services;
        }
    }
}
