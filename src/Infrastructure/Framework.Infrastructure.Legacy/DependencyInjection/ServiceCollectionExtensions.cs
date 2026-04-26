using System.Reflection;

using Anch.DependencyInjection;

using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Services;
using Framework.BLL.Validation;
using Framework.BLL.Visitors;
using Framework.Core;
using Framework.Database;
using Framework.ExtendedMetadata;
using Framework.Infrastructure.ApiControllerBaseEvaluator;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Infrastructure.Services;
using Framework.Infrastructure.WebApiExceptionExpander;
using Framework.Projection;
using Framework.Tracking;
using Framework.Validation;

using Anch.HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using Anch.OData.DependencyInjection;

using Anch.SecuritySystem;
using Anch.SecuritySystem.DomainServices;
using Anch.SecuritySystem.DomainServices.DependencySecurity;
using Anch.SecuritySystem.SecurityRuleInfo;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLegacyGenericServices()
        {
            services.AddSingleton<ITargetSystemTypeResolverContainer, TargetSystemTypeResolverContainer>();
            services.AddSingleton<ITargetSystemInfoService, TargetSystemInfoService>();

            services.AddScoped<IRootSecurityService, RootSecurityService>();

            services.AddSingleton<IMetadataProxyProvider, MetadataProxyProvider>();
            services.AddSingleton<IPropertyPathService, PropertyPathService>();

            services.AddScoped(typeof(EvaluatedData<,>));
            services.AddKeyedScoped<IEventOperationSender, BLLEventOperationSender>(nameof(BLL));

            services.AddSingleton<IAvailableValues>(AvailableValuesHelper.AvailableValues.ToValidation());
            services.AddScoped(typeof(ITrackingService<>), typeof(TrackingService<>));

            services.ReplaceSingleton<IActualDomainTypeResolver, ProjectionActualDomainTypeResolver>();
            services.ReplaceSingleton<ISecurityContextInfoSource, ProjectionSecurityContextInfoSource>();

            services.AddKeyedSingleton<IExceptionExpander, TargetInvocationExceptionExpander>(IExceptionExpander.ElementKey);

            services.AddSingleton(new WebApiExceptionExpanderSettings([typeof(ValidationException)]));

            services.AddKeyedSingleton<IExpressionVisitorContainer, ExpandPathVisitorContainer>(IExpressionVisitorContainer.ElementKey);

            services.AddOData();

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
