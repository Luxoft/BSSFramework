using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModelGenerator;

internal static class GeneratorConfigurationExtensions
{
    public static IEnumerable<Type> GetActualDomainTypes(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return configuration.DomainTypes.Where(configuration.HasMethods);
    }

    public static IEnumerable<IServiceMethodGenerator> GetActualMethodGenerators(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, Type domainType)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return configuration.GetMethodGenerators(domainType).Where(m => configuration.GeneratePolicy.Used(domainType, m.Identity));
    }

    public static CodeExpression GetByIdExpr(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

        return configuration.GetByIdExprByIdentityRef(bllRefExpr, parameterExprRef.ToPropertyReference(configuration.Environment.IdentityProperty), fetchsExpr);
    }

    public static CodeExpression GetByIdExprByIdentityRef(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterIdentityProperty, CodeExpression fetchsExpr)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterIdentityProperty == null) throw new ArgumentNullException(nameof(parameterIdentityProperty));

        return bllRefExpr.ToMethodInvokeExpression("GetById", parameterIdentityProperty, new CodePrimitiveExpression(true), fetchsExpr);
    }

    public static CodeExpression GetByIdExpr(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterExprRef)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

        return configuration.GetByIdExprByIdentityRef(bllRefExpr, parameterExprRef.ToPropertyReference(configuration.Environment.IdentityProperty));
    }

    public static CodeExpression GetByIdExprByIdentityRef(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterIdentityProperty)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterIdentityProperty == null) throw new ArgumentNullException(nameof(parameterIdentityProperty));

        return bllRefExpr.ToMethodInvokeExpression("GetById", parameterIdentityProperty, new CodePrimitiveExpression(true));
    }

    public static CodeExpression GetByNameExpr(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

        var method = typeof(DefaultDomainBLLBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetByName");

        return bllRefExpr.ToStaticMethodInvokeExpression(method, parameterExprRef, new CodePrimitiveExpression(true), fetchsExpr);
    }

    public static CodeExpression GetByCodeExpr(this IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration, CodeExpression bllRefExpr, CodeExpression parameterExprRef, CodeExpression fetchsExpr)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameterExprRef == null) throw new ArgumentNullException(nameof(parameterExprRef));

        var method = typeof(DefaultDomainBLLBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("GetByCode");

        return bllRefExpr.ToStaticMethodInvokeExpression(method, parameterExprRef, new CodePrimitiveExpression(true), fetchsExpr);
    }

    public static CodeExpression GetFetchContainerExpr(this CodeExpression contextExpr, Type domainType, CodeExpression[] fetchParams)
    {
        if (contextExpr == null) throw new ArgumentNullException(nameof(contextExpr));
        if (fetchParams == null) throw new ArgumentNullException(nameof(fetchParams));

        return contextExpr.ToPropertyReference("FetchService")
                          .ToMethodReferenceExpression("GetContainer", domainType.ToTypeReference())
                          .ToMethodInvokeExpression(fetchParams);
    }
}
