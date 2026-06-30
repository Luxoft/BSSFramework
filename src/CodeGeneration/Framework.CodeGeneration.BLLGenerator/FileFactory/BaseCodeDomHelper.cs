using System.CodeDom;

using Anch.SecuritySystem.Providers;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.Core;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

internal static class BaseCodeDomHelper
{
    private const string ContextParameterNameBase = "context";

    extension<TConfiguration>(FileFactory<TConfiguration> fileFactory)
        where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
    {
        public CodeParameterDeclarationExpression GetContextParameter()
        {
            if (fileFactory is null) throw new ArgumentNullException(nameof(fileFactory));

            return fileFactory.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression(ContextParameterNameBase);
        }

        public CodeParameterDeclarationExpression GetSecurityProviderParameter(CodeTypeParameter domainObjectParameter)
        {
            if (fileFactory is null) throw new ArgumentNullException(nameof(fileFactory));
            if (domainObjectParameter is null) throw new ArgumentNullException(nameof(domainObjectParameter));

            return typeof(ISecurityProvider<>).ToTypeReference(domainObjectParameter.ToTypeReference())
                                              .ToParameterDeclarationExpression("securityProvider");
        }

        public CodeTypeParameter GetDomainObjectCodeTypeParameter(bool withConstraints = true)
        {
            if (fileFactory is null) throw new ArgumentNullException(nameof(fileFactory));

            return new CodeTypeParameter { Name = "TDomainObject" }.Self(withConstraints, p =>

                                                                             p.Constraints.Add(fileFactory.Configuration.Environment.PersistentDomainObjectBaseType));
        }

        public CodeTypeParameter GetSecurityModeCodeTypeParameter()
        {
            if (fileFactory is null) throw new ArgumentNullException(nameof(fileFactory));

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

