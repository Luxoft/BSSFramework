using System.Reflection;
using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Extensions;
using Framework.Core;
using Framework.Infrastructure.ApiControllerBaseEvaluator;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Projection;

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
        public IServiceCollection RegisterContextEvaluators(bool useSingleCall = false)
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

        public IServiceCollection RegisterProjectionDomainSecurityServices(Assembly assembly)
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
