using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

using SecuritySystem.Providers;

namespace Framework.DomainDriven.BLLGenerator;

internal static class BaseCodeDomHelper
{
    private const string ContextParameterNameBase = "context";

    extension<TConfiguration>(FileFactory<TConfiguration> fileFactory)
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public CodeParameterDeclarationExpression GetContextParameter()
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return fileFactory.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression(ContextParameterNameBase);
        }

        public CodeParameterDeclarationExpression GetSecurityProviderParameter(CodeTypeParameter domainObjectParameter)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));
            if (domainObjectParameter == null) throw new ArgumentNullException(nameof(domainObjectParameter));

            return typeof(ISecurityProvider<>).ToTypeReference(domainObjectParameter.ToTypeReference())
                                              .ToParameterDeclarationExpression("securityProvider");
        }

        public CodeTypeParameter GetDomainObjectCodeTypeParameter(bool withConstraints = true)
        {
            if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

            return new CodeTypeParameter { Name = "TDomainObject" }.Self(withConstraints, p =>

                                                                             p.Constraints.Add(fileFactory.Configuration.Environment.PersistentDomainObjectBaseType));
        }

        public CodeTypeParameter GetSecurityModeCodeTypeParameter()
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
}
