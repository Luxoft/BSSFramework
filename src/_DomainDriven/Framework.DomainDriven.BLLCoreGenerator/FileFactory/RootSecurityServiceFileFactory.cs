using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class RootSecurityServiceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public RootSecurityServiceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.RootSecurityService;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var contextTypeRef = this.Configuration.BLLContextInterfaceTypeReference;

        var contextParameter = contextTypeRef.ToParameterDeclarationExpression("context");
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var ctor = new CodeConstructor
                   {
                           Attributes = MemberAttributes.Public,
                           Parameters = { contextParameter },
                           BaseConstructorArgs = { contextParameterRefExpr }
                   };

        return new CodeTypeDeclaration
               {
                       BaseTypes =
                       {
                               this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServiceBase),
                               this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServiceInterface),
                       },
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true,
                       Members = { ctor }
               };
    }
}
