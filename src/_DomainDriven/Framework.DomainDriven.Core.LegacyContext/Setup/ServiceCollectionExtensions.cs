using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Tracking;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBLLSystem<TBLLContextDecl, TBLLContextImpl>(
        this IServiceCollection services,
        Action<BLLSystemSettings> setupAction = null)
        where TBLLContextImpl : TBLLContextDecl
    {
        var settings = ExtractSettings<TBLLContextDecl, TBLLContextImpl>();

        setupAction?.Invoke(settings);

        return typeof(ServiceCollectionExtensions)
               .GetMethod(nameof(RegisterBLLSystemInternal), BindingFlags.NonPublic | BindingFlags.Static, true)
               .MakeGenericMethod(
               [
                   typeof(TBLLContextDecl),
                   typeof(TBLLContextImpl),
                   settings.GetSafe(v => v.PersistentDomainObjectBaseType),
                   settings.GetSafe(v => v.ValidationMapType, typeof(IValidationMap)),
                   settings.GetSafe(v => v.ValidatorCompileCacheType, typeof(ValidatorCompileCache)),
                   settings.GetSafe(v => v.ValidatorDeclType),
                   settings.GetSafe(v => v.ValidatorImplType, typeof(SuccessValidator)),
                   settings.GetSafe(v => v.FactoryContainerDeclType),
                   settings.GetSafe(v => v.FactoryContainerImplType),
                   settings.GetSafe(v => v.SettingsType)
               ])
               .Invoke<IServiceCollection>(null, [services]);
    }

    private static IServiceCollection RegisterBLLSystemInternal<
        TBLLContextDecl,
        TBLLContextImpl,
        TPersistentDomainObjectBase,
        TValidationMap,
        TValidatorCompileCache,
        TValidatorDecl,
        TValidatorImpl,
        TBLLFactoryContainerDecl,
        TBLLFactoryContainerImpl,
        TBLLContextSettings>(this IServiceCollection services)
        where TBLLContextImpl : class, TBLLContextDecl

        where TValidationMap : class, IValidationMap
        where TValidatorCompileCache : ValidatorCompileCache
        where TValidatorImpl : class, TValidatorDecl
        where TValidatorDecl : class

        where TBLLFactoryContainerDecl : class
        where TBLLFactoryContainerImpl : class, TBLLFactoryContainerDecl, IBLLFactoryInitializer
        where TBLLContextSettings : class
        where TBLLContextDecl : class
    {
        if (typeof(TValidationMap) != typeof(IValidationMap))
        {
            services.AddSingleton<TValidationMap>();
        }

        if (typeof(TValidatorCompileCache) != typeof(ValidatorCompileCache))
        {
            services.AddSingleton<TValidatorCompileCache>();
        }

        services.AddScoped<TValidatorDecl, TValidatorImpl>()
                .AddScoped<TBLLFactoryContainerDecl, TBLLFactoryContainerImpl>()
                .AddSingleton<TBLLContextSettings>()
                .AddScopedFromLazyInterfaceImplement<TBLLContextDecl, TBLLContextImpl>()

                .Self(TBLLFactoryContainerImpl.RegisterBLLFactory);

        return services;
    }

    private static BLLSystemSettings ExtractSettings<TBLLContextDecl, TBLLContextImpl>()
        where TBLLContextImpl : TBLLContextDecl
    {
        var asmTypes = new[] { typeof(TBLLContextDecl).Assembly, typeof(TBLLContextImpl).Assembly }.Distinct()
            .SelectMany(assembly => assembly.GetTypes())
            .ToList();

        var validatorDeclType = typeof(TBLLContextImpl).GetSingleCtorParameterTypeImpl(typeof(IValidator));

        var validatorImplType = GetImplType(validatorDeclType, false);

        var validatorCompileCacheType = validatorImplType?.GetSingleCtorParameterTypeImpl(typeof(ValidatorCompileCache));

        var validationMapType = validatorCompileCacheType?.GetSingleCtorParameterTypeImpl(typeof(IValidationMap));

        var persistentDomainObjectBaseType = typeof(TBLLContextDecl).GetInterfaceImplementationArgument(typeof(ITrackingServiceContainer<>))!;

        var baseBllSettingType = typeof(BLLContextSettings<>).MakeGenericType(persistentDomainObjectBaseType);

        var factoryContainerDeclType = typeof(TBLLContextImpl).GetSingleCtorParameterTypeImpl(typeof(IBLLFactoryContainer<object>));

        return new BLLSystemSettings
               {
                   PersistentDomainObjectBaseType = persistentDomainObjectBaseType,
                   ValidatorDeclType = validatorDeclType,
                   ValidatorImplType = validatorImplType,
                   ValidatorCompileCacheType = validatorCompileCacheType,
                   ValidationMapType = validationMapType,
                   SettingsType = GetImplType(baseBllSettingType, false) ?? baseBllSettingType,
                   FactoryContainerDeclType = factoryContainerDeclType,
                   FactoryContainerImplType = GetImplType(factoryContainerDeclType, true)
               };

        Type GetImplType(Type baseType, bool required)
        {
            return asmTypes.SingleOrDefault(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t))
                           .Pipe(required, res => res ?? throw new Exception($"Implement type for '{baseType}' not found"));
        }
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
