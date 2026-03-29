using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.Core;
using Framework.Database;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

public class AddAttachmentMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType) : FileStoreMethodGeneratorBase<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.AddAttachment;


    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    protected override string Name => $"Add{this.DomainType.Name}Attachment";

    protected override bool IsEdit { get; } = true;

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, DTOType.IdentityDTO);

    private CodeParameterDeclarationExpression FileItemParameter => new(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.FileItemType, DTOType.StrictDTO), "fileItemDTO");


    protected override string GetComment() => $"Add {this.DomainType.Name} attachment";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        var codeTypeReference = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

        yield return codeTypeReference.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.FileItemParameter;
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var fileItem = this.Configuration.FileItemType.ToTypeReference().ToVariableDeclarationStatement("fileItem");

        yield return fileItem;

        var method = this.FileItemParameter.ToVariableReferenceExpression()
                         .ToMethodInvokeExpression(
                                                   this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName,
                                                   evaluateDataExpr.GetMappingService(),
                                                   new CodePrimitiveExpression(true));


        yield return method.ToAssignStatement(fileItem.ToVariableReferenceExpression());


        var domainObjectDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectDecl;

        var objectFileContainerDecl = this.GetDefaultBLLVariableDeclaration(evaluateDataExpr, "objectFileContainerBLL", this.Configuration.ObjectFileContainerType);

        yield return objectFileContainerDecl;

        yield return objectFileContainerDecl.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.AddFileItemMethodName, domainObjectDecl.ToVariableReferenceExpression(), fileItem.ToVariableReferenceExpression()).ToExpressionStatement();

        yield return fileItem.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                             .ToMethodReturnStatement();
    }
}
