using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator;

internal static class BaseCodeDomHelper
{
    private const string ContextParameterNameBase = "context";

    private const string SecurityOperationNameBase = "securityOperation";

    public static CodeParameterDeclarationExpression GetContextParameter<TConfiguration>(this FileFactory<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return fileFactory.Configuration.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression(ContextParameterNameBase);
    }

    public static CodeParameterDeclarationExpression GetSecurityProviderParameter<TConfiguration>(this FileFactory<TConfiguration> fileFactory, CodeTypeParameter domainObjectParameter)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
        if (domainObjectParameter == null) throw new ArgumentNullException(nameof(domainObjectParameter));

        return typeof(ISecurityProvider<>).ToTypeReference(domainObjectParameter.ToTypeReference())
                                          .ToParameterDeclarationExpression(SecurityOperationNameBase);
    }

    public static CodeTypeParameter GetDomainObjectCodeTypeParameter<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory, bool withConstraints = true)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return new CodeTypeParameter { Name = "TDomainObject" }.Self(withConstraints, p =>

                                                                                              p.Constraints.Add(fileFactory.Configuration.Environment.PersistentDomainObjectBaseType));
    }

    public static CodeTypeParameter GetOperationCodeTypeParameter<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return new CodeTypeParameter
               {
                       Name = "TOperation",
                       Constraints =
                       {
                               new CodeTypeReference(" struct"),
                               typeof(Enum).ToTypeReference()
                       }
               };
    }

    public static CodeTypeParameter GetSecurityModeCodeTypeParameter<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return new CodeTypeParameter
               {
                       Name = "TSecurityMode",
                       Constraints =
                       {
                               new CodeTypeReference(" struct")
                       }
               };
    }
}
