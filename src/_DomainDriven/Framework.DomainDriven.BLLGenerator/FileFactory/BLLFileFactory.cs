using System.CodeDom;

using Framework.CodeDom;

using SecuritySystem.Providers;

namespace Framework.DomainDriven.BLLGenerator;

public class BLLFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

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
