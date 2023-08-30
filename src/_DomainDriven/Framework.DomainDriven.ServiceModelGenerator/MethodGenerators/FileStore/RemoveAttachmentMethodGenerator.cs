using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.FileStore;

public class RemoveAttachmentMethodGenerator<TConfiguration> : FileStoreMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public RemoveAttachmentMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.RemoveAttachment;


    protected override string Name => $"Remove{this.DomainType.Name}Attachment";

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    private CodeParameterDeclarationExpression FileItemParameter =>

            new CodeParameterDeclarationExpression(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, Transfering.DTOType.IdentityDTO), "fileItemDTO");

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    protected override bool IsEdit { get; } = true;


    protected override string GetComment()
    {
        return $"Remove {this.DomainType.Name} attachment";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        var codeTypeReference = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, Transfering.DTOType.IdentityDTO);

        yield return codeTypeReference.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.FileItemParameter;
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectDecl;

        var objectFileContainerDecl = this.GetDefaultBLLVariableDeclaration(evaluateDataExpr, "objectFileContainerBLL", this.Configuration.ObjectFileContainerType);

        yield return objectFileContainerDecl;

        yield return objectFileContainerDecl.ToVariableReferenceExpression()
                                            .ToMethodInvokeExpression(this.Configuration.RemoveFileItemMethodName,
                                                                      domainObjectDecl.ToVariableReferenceExpression(), this.FileItemParameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty))
                                            .ToExpressionStatement();


    }
}
