using System.Reflection;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBLLSystem<TBLLContextDecl, TBLLContextImpl>(
        this IServiceCollection services,
        Action<BLLSystemSettings> setup = null)
        where TBLLContextImpl : TBLLContextDecl
    {
        var settings = ExtractSettings<TBLLContextDecl, TBLLContextImpl>();

        setup?.Invoke(settings);

        return typeof(ServiceCollectionExtensions)
               .GetMethod(nameof(RegisterBLLSystemInternal), BindingFlags.NonPublic | BindingFlags.Static, true)
               .MakeGenericMethod(
               [
                   settings.GetSafe(v => v.PersistentDomainObjectBaseType),
                   typeof(TBLLContextDecl),
                   typeof(TBLLContextImpl),
                   settings.GetSafe(v => v.ValidationMapType),
                   settings.GetSafe(v => v.ValidatorCompileCacheType),
                   settings.GetSafe(v => v.ValidatorDeclType),
                   settings.GetSafe(v => v.ValidatorImplType),
                   settings.GetSafe(v => v.FetchServiceType),
                   settings.GetSafe(v => v.FactoryContainerDeclType),
                   settings.GetSafe(v => v.FactoryContainerImplType),
                   settings.GetSafe(v => v.SettingsType)
               ])
               .Invoke<IServiceCollection>(null, [services]);
    }

    private static IServiceCollection RegisterBLLSystemInternal<
        TPersistentDomainObjectBase,
        TBLLContextDecl,
        TBLLContextImpl,
        TValidationMap,
        TValidatorCompileCache,
        TValidatorDecl,
        TValidatorImpl,
        TMainFetchService,
        TBLLFactoryContainerImpl,
        TBLLFactoryContainerDecl,
        TBLLContextSettings>(this IServiceCollection services)
        where TBLLContextImpl : class, TBLLContextDecl
        where TValidationMap : class
        where TValidatorCompileCache : ValidatorCompileCache
        where TValidatorImpl : class, TValidatorDecl
        where TValidatorDecl : class
        where TMainFetchService : MainFetchServiceBase<TPersistentDomainObjectBase>, new()
        where TBLLFactoryContainerDecl : class
        where TBLLFactoryContainerImpl : class, TBLLFactoryContainerDecl, IBLLFactoryInitializer
        where TBLLContextSettings : BLLContextSettings<TPersistentDomainObjectBase>
        where TBLLContextDecl : class
    {
        return services
               .AddSingleton<TValidationMap>()
               .AddSingleton<TValidatorCompileCache>()

               .AddScoped<TValidatorDecl, TValidatorImpl>()

               .AddSingleton(
                   new TMainFetchService().WithCompress().WithCache().WithLock()
                                          .Add(FetchService<TPersistentDomainObjectBase>.OData))
               .AddScoped<TBLLFactoryContainerDecl, TBLLFactoryContainerImpl>()
               .AddSingleton<TBLLContextSettings>()
               .AddScopedFromLazyInterfaceImplement<TBLLContextDecl, TBLLContextImpl>()

               .Self(TBLLFactoryContainerImpl.RegisterBLLFactory);
    }

    private static BLLSystemSettings ExtractSettings<TBLLContextDecl, TBLLContextImpl>()
        where TBLLContextImpl : TBLLContextDecl
    {
        var asmTypes = new[] { typeof(TBLLContextDecl).Assembly, typeof(TBLLContextImpl).Assembly }.Distinct()
            .SelectMany(assembly => assembly.GetTypes())
            .ToList();

        var validatorDeclType = typeof(TBLLContextImpl).GetSingleCtorParameterTypeImpl(typeof(IValidator));

        var validatorImplType = asmTypes.Single(t => t.IsClass && validatorDeclType.IsAssignableFrom(t));

        var validatorCompileCacheType = validatorImplType.GetSingleCtorParameterTypeImpl(typeof(ValidatorCompileCache));

        var validationMapType = validatorCompileCacheType.GetSingleCtorParameterTypeImpl(typeof(ValidationMapBase));

        var persistentDomainObjectBaseType = typeof(TBLLContextDecl)
                                             .GetInterfaceImplementationArguments(typeof(IFetchServiceContainer<,>)).First();

        var baseBllSettingType = typeof(BLLContextSettings<>).MakeGenericType(persistentDomainObjectBaseType);

        var factoryContainerDeclType = typeof(TBLLContextDecl)
                                       .GetInterfaceImplementationArgument(typeof(IBLLFactoryContainerContext<>));

        return new BLLSystemSettings
               {
                   PersistentDomainObjectBaseType = persistentDomainObjectBaseType,
                   ValidatorDeclType = validatorDeclType,
                   ValidatorImplType = validatorImplType,
                   ValidatorCompileCacheType = validatorCompileCacheType,
                   ValidationMapType = validationMapType,
                   FetchServiceType = typeof(TBLLContextDecl).GetInterfaceImplementation(typeof(IFetchServiceContainer<,>)),
                   SettingsType = asmTypes.SingleOrDefault(t => t.IsClass && baseBllSettingType.IsAssignableFrom(t)) ?? baseBllSettingType,
                   FactoryContainerDeclType = factoryContainerDeclType,
                   FactoryContainerImplType = asmTypes.Single(t => t.IsClass && factoryContainerDeclType.IsAssignableFrom(t))
        };
    }


    private static Type GetSingleCtorParameterTypeImpl(this Type type, Type implType)
    {
        return type.GetSingleCtorParameterTypes().Single(implType.IsAssignableFrom);
    }

    private static IEnumerable<Type> GetSingleCtorParameterTypes(this Type type)
    {
        return type.GetConstructors().Single().GetParameters().Select(p => p.ParameterType);
    }
}
