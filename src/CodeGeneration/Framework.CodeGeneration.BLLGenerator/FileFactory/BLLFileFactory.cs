using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;

using SecuritySystem.Providers;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class BLLFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IbllGeneratorConfiguration<IbllGenerationEnvironment>
{
    public override FileType FileType => FileType.BLL;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var baseBLLType = this.Configuration.GetSecurityDomainBLLBaseTypeReference(this.DomainType!)
                              .ToTypeReference(this.DomainType!.ToTypeReference());

        var codeTypeDeclaration = new CodeTypeDeclaration
                                  {
                                          Name = this.Name,

                                          Attributes = MemberAttributes.Public,
                                          IsPartial = true,

                                          BaseTypes =
                                          {
                                                  baseBLLType,

                                                  this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLInterface)
                                          }
                                  };

        {
            if (this.Configuration.GenerateBllConstructor(this.DomainType))
            {
                var contextParameter = new CodeParameterDeclarationExpression
                                       {
                                               Type = this.Configuration.BLLContextTypeReference,
                                               Name = "context"
                                       };
                var contextParameterExpr = contextParameter.ToVariableReferenceExpression();

                var securityProviderParameterTypeRef = typeof(ISecurityProvider<>).ToTypeReference(this.DomainType.ToTypeReference());
                var securityProviderParameter = securityProviderParameterTypeRef.ToParameterDeclarationExpression("securityProvider");

                var securityOperationConstructor = new CodeConstructor
                                                   {
                                                           Attributes = MemberAttributes.Public,
                                                           Parameters =
                                                           {
                                                                   contextParameter,
                                                                   securityProviderParameter
                                                           },
                                                           BaseConstructorArgs =
                                                           {
                                                                   contextParameterExpr,
                                                                   securityProviderParameter.ToVariableReferenceExpression()
                                                           }
                                                   };

                codeTypeDeclaration.Members.Add(securityOperationConstructor);
            }
        }

        return codeTypeDeclaration;
    }
}
