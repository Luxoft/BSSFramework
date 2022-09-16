using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection;

public static class ServiceCollectionValidationExtensions
{
    public static IServiceCollection ValidateDuplicateDeclaration(this IServiceCollection serviceCollection)
    {
        var wrongMultiUsage = serviceCollection.GetWrongMultiUsage();

        if (wrongMultiUsage.Any())
        {
            var message = wrongMultiUsage.Join(
                Environment.NewLine,
                pair => $"The service {pair.ServiceType} has been registered many times. There are services that use it in the constructor in a single instance: "
                        + pair.UsedFor.Join(", ", usedService => usedService.ImplementationType));

            throw new InvalidOperationException(message);
        }

        return serviceCollection;
    }

    private static List<(Type ServiceType, List<ServiceDescriptor> UsedFor)> GetWrongMultiUsage(this IServiceCollection serviceCollection)
    {
        var usedParametersRequest =

                from service in serviceCollection

                where service.ImplementationType != null && service.ImplementationFactory == null

                let ctors = service.ImplementationType!.GetConstructors()

                let actualCtor = ctors.Length == 1
                                         ? ctors[0]
                                         : ctors
                                           .Where(ctor => ctor.HasAttribute<ActivatorUtilitiesConstructorAttribute>())
                                           .Match(() => null, ctor => ctor, _ => null)

                where actualCtor != null

                from parameterType in actualCtor.GetParameters().Select(p => p.ParameterType).Distinct()

                group service by parameterType;

        var usedParametersDict = usedParametersRequest.ToDictionary(g => g.Key, g => g.ToList());



        var wrongMultiUsageRequest =

                from service in serviceCollection

                group service by service.ServiceType into serviceTypeGroup

                where serviceTypeGroup.Count() > 1

                let servicesWithSimpleUsage = usedParametersDict.GetMaybeValue(serviceTypeGroup.Key).GetValueOrDefault()

                where servicesWithSimpleUsage != null

                select (ServiceType: serviceTypeGroup.Key, UsedFor: servicesWithSimpleUsage);

        return wrongMultiUsageRequest.ToList();
    }
}
