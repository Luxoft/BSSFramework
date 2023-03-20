using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class DomainBLLBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DomainBLLBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.DomainBLLBase;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
        var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

        var genericOperationParameter = this.GetOperationCodeTypeParameter();
        var genericOperationParameterTypeRef = genericOperationParameter.ToTypeReference();

        var contextParameter = this.GetContextParameter();
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var specificationEvaluatorParameterTypeRef = typeof(ISpecificationEvaluator).ToTypeReference();
        var specificationEvaluatorParameter = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator = null");
        var specificationEvaluatorParameterArg = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator").ToVariableReferenceExpression();


        return new CodeTypeDeclaration
               {
                       TypeParameters =
                       {
                               genericDomainObjectParameter, genericOperationParameter
                       },

                       Name = this.Name,
                       IsClass = true,
                       IsPartial = true,
                       BaseTypes =
                       {
                               typeof(DefaultDomainBLLBase<,,,,,>).ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference,
                                                                                   this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                                                                   this.Configuration.Environment.DomainObjectBaseType.ToTypeReference(),
                                                                                   genericDomainObjectParameterTypeRef,
                                                                                   this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                                                                                   genericOperationParameterTypeRef)
                       },
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Public,
                                       Parameters = { contextParameter, specificationEvaluatorParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr, specificationEvaluatorParameterArg }
                               }
                       }
               };
    }
}
