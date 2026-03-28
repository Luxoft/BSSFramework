using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;
using Framework.Core;
using Framework.Database;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

public class RemoveAttachmentMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType)
    : FileStoreMethodGeneratorBase<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.RemoveAttachment;


    protected override string Name => $"Remove{this.DomainType.Name}Attachment";

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    private CodeParameterDeclarationExpression FileItemParameter =>

            new(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, DTOType.IdentityDTO), "fileItemDTO");

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    protected override bool IsEdit { get; } = true;


    protected override string GetComment() => $"Remove {this.DomainType.Name} attachment";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        var codeTypeReference = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

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
