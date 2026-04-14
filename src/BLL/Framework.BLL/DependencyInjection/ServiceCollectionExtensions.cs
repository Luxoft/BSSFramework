using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.DependencyInjection;
using Framework.Validation;
using Framework.Validation.Map;

using GenericQueryable.Fetching;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBLLSystem<TBLLContextDecl, TBLLContextImpl>(
        this IServiceCollection services,
        Action<BLLSystemSettings>? setupAction = null)
        where TBLLContextImpl : TBLLContextDecl
    {
        var settings = ExtractSettings<TBLLContextDecl, TBLLContextImpl>();

        setupAction?.Invoke(settings);

        return typeof(ServiceCollectionExtensions)
               .GetMethod(nameof(AddBLLSystemInternal), BindingFlags.NonPublic | BindingFlags.Static, true)!
               .MakeGenericMethod(
                   typeof(TBLLContextDecl),
                   typeof(TBLLContextImpl),
                   settings.GetSafe(v => v.ValidationMapType, typeof(IValidationMap)),
                   settings.GetSafe(v => v.ValidatorCompileCacheType, typeof(ValidatorCompileCache)),
                   settings.GetSafe(v => v.ValidatorDeclType),
                   settings.GetSafe(v => v.ValidatorImplType, typeof(SuccessValidator)),
                   settings.GetSafe(v => v.FactoryContainerDeclType),
                   settings.GetSafe(v => v.FactoryContainerImplType),
                   settings.GetSafe(v => v.FetchRuleExpanderType, typeof(IFetchRuleExpander)))
               .Invoke<IServiceCollection>(null, [services]);
    }

    private static IServiceCollection AddBLLSystemInternal<
        TBLLContextDecl,
        TBLLContextImpl,
        TValidationMap,
        TValidatorCompileCache,
        TValidatorDecl,
        TValidatorImpl,
        TBLLFactoryContainerDecl,
        TBLLFactoryContainerImpl,
        TFetchRuleExpander>(this IServiceCollection services)
        where TBLLContextImpl : class, TBLLContextDecl

        where TValidationMap : class, IValidationMap
        where TValidatorCompileCache : ValidatorCompileCache
        where TValidatorImpl : class, TValidatorDecl
        where TValidatorDecl : class

        where TBLLFactoryContainerDecl : class
        where TBLLFactoryContainerImpl : class, TBLLFactoryContainerDecl, IBLLFactoryInitializer
        where TBLLContextDecl : class

        where TFetchRuleExpander : class, IFetchRuleExpander
    {
        if (typeof(TValidationMap) != typeof(IValidationMap))
        {
            services.AddSingleton<TValidationMap>();
        }

        if (typeof(TValidatorCompileCache) != typeof(ValidatorCompileCache))
        {
            services.AddSingleton<TValidatorCompileCache>();
        }

        if (typeof(TFetchRuleExpander) != typeof(IFetchRuleExpander))
        {
            services.AddKeyedSingleton<IFetchRuleExpander, TFetchRuleExpander>(IFetchRuleExpander.ElementKey);
        }

        services.AddScoped<TValidatorDecl, TValidatorImpl>()
                .AddScoped<TBLLFactoryContainerDecl, TBLLFactoryContainerImpl>()
                .AddScopedFromLazyInterfaceImplement<TBLLContextDecl, TBLLContextImpl>()

                .Self(TBLLFactoryContainerImpl.RegisterBLLFactory);

        return services;
    }

    private static BLLSystemSettings ExtractSettings<TBLLContextDecl, TBLLContextImpl>()
        where TBLLContextImpl : TBLLContextDecl
    {
        var asmTypes = new[] { typeof(TBLLContextDecl).Assembly, typeof(TBLLContextImpl).Assembly }
                       .Distinct().SelectMany(assembly => assembly.GetTypes()).ToList();

        var validatorDeclType = typeof(TBLLContextImpl).GetSingleCtorParameterTypeImpl(typeof(IValidator));

        var validatorImplType = TryGetImplType(validatorDeclType);

        var validatorCompileCacheType = validatorImplType?.GetSingleCtorParameterTypeImpl(typeof(ValidatorCompileCache));

        var validationMapType = validatorCompileCacheType?.GetSingleCtorParameterTypeImpl(typeof(IValidationMap));

        var persistentDomainObjectBaseType = typeof(TBLLContextDecl).GetInterfaceImplementationArguments(typeof(IDefaultBLLContext<,>), args => args.First())!;

        var factoryContainerDeclType = typeof(TBLLContextImpl).GetSingleCtorParameterTypeImpl(typeof(IBLLFactoryContainer<object>));

        return new BLLSystemSettings
               {
                   ValidatorDeclType = validatorDeclType,
                   ValidatorImplType = validatorImplType,
                   ValidatorCompileCacheType = validatorCompileCacheType,
                   ValidationMapType = validationMapType,
                   FactoryContainerDeclType = factoryContainerDeclType,
                   FactoryContainerImplType = GetImplType(factoryContainerDeclType),
                   FetchRuleExpanderType = TryGetImplType(typeof(DTOFetchRuleExpander<>).MakeGenericType(persistentDomainObjectBaseType)) ?? typeof(IFetchRuleExpander),
               };

        Type? TryGetImplType(Type baseType) => asmTypes.SingleOrDefault(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t));

        Type GetImplType(Type baseType) => TryGetImplType(baseType) ?? throw new Exception($"Implement type for '{baseType}' not found");
    }


    private static Type GetSingleCtorParameterTypeImpl(this Type type, Type implType) => type.GetSingleCtorParameterTypes().Single(implType.IsAssignableFrom);

    private static IEnumerable<Type> GetSingleCtorParameterTypes(this Type type) => type.GetConstructors().Single().GetParameters().Select(p => p.ParameterType);
}
