using System.CodeDom;

using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStoreAttachment;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

public class GetAttachmentWebGetMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IFileStoreAttachmentGeneratorConfiguration<IFileStoreAttachmentGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.GetAttachment;

    protected override string Name => $"Get{this.DomainType.Name}Attachment";

    protected override CodeTypeReference ReturnType { get; } = new(typeof(Stream));

    protected override bool IsEdit { get; } = false;

    protected override string GetComment() => $"Get {this.DomainType.Name} attachment";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters() =>
    [
        new CodeParameterDeclarationExpression(typeof(Guid), "id"),
        new CodeParameterDeclarationExpression(typeof(Guid), "randomGuid")
    ];

    public override CodeMemberMethod GetContractMethod()
    {
        var baseResult = base.GetContractMethod();

        var tail = "attachment?id={id}&randomGuid={randomGuid}";

        var webGetAttributeValue = $"/{this.Configuration.WebGetPath}/{this.DomainType.Name}/{tail}";

        var webGetAttribute = new CodePrimitiveExpression(webGetAttributeValue);

        var webGetTypeReference = typeof(WebGetAttribute).ToTypeReference();

        baseResult.CustomAttributes.Add(
                                        webGetTypeReference.ToAttributeDeclaration(new CodeAttributeArgument("UriTemplate", webGetAttribute)));

        return baseResult;
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var opearationCode = this.Configuration.TryGetSecurityAttribute(this.DomainType, this.IsEdit);
        if (null != opearationCode)
        {
            return opearationCode;
        }

        return base.GetBLLSecurityParameter(evaluateDataExpr);
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var fileItemBLLDecl = this.GetCreateDefaultBLLVariableDeclaration(evaluateDataExpr, "fileItemBLL", this.Configuration.FileItemType);

        yield return fileItemBLLDecl;

        var fileItemBLLRef = fileItemBLLDecl.ToVariableReferenceExpression();

        var fileItemRef = this.Configuration.GetByIdExprByIdentityRef(fileItemBLLRef, this.Parameters[0].ToVariableReferenceExpression());

        var fileItemDecl = new CodeVariableDeclarationStatement(this.Configuration.FileItemType, "fileItem", fileItemRef);

        yield return fileItemDecl;

        {
            var fileItemContainerLinkBLLDecl = this.GetCreateDefaultBLLVariableDeclaration(evaluateDataExpr, "fileItemContainerLinkBLL", this.Configuration.FileItemContainerLinkType);

            yield return fileItemContainerLinkBLLDecl;

            var getQueryableMethod = fileItemContainerLinkBLLDecl
                                     .ToVariableReferenceExpression()
                                     .ToMethodInvokeExpression("GetUnsecureQueryable");

            var enumerable = new CodeTypeReferenceExpression(typeof(Queryable));

            var getFileItemContainerLink =
                    enumerable.ToMethodInvokeExpression(
                                                        "FirstOrDefault",
                                                        enumerable.ToMethodInvokeExpression(
                                                         "Where",
                                                         getQueryableMethod,
                                                         new CodeSnippetExpression($"z=>z.FileItem.Id == {this.Parameters[0].Name}")));

            var fileItemContainerLink =
                    new CodeVariableDeclarationStatement(
                                                         this.Configuration.FileItemContainerLinkType,
                                                         "fileItemContainerLink",
                                                         getFileItemContainerLink);

            yield return fileItemContainerLink;

            var binary = new CodeIsNullExpression(fileItemContainerLink.ToVariableReferenceExpression());

            var fileItemLinkContainerLinkIsNull = new CodeConditionStatement(
                                                                             binary,
                                                                             new CodeThrowArgumentOutOfRangeExceptionStatement(this.Parameters[0]));

            yield return fileItemLinkContainerLinkIsNull;

            var objectFileContainerPropertyName = this.Configuration.FileItemContainerLinkType.GetProperties().First(z => z.PropertyType == this.Configuration.ObjectFileContainerType).Name;
            var domainObjectIndentPropertyName =
                    this.Configuration.ObjectFileContainerType.GetProperties()
                        .Where(z => z.PropertyType == this.Configuration.Environment.IdentityProperty.PropertyType)
                        .First(z => z.Name != this.Configuration.Environment.IdentityProperty.Name);

            var objectIdentProperty =
                    fileItemContainerLink.ToVariableReferenceExpression()
                                         .ToPropertyReference(objectFileContainerPropertyName)
                                         .ToPropertyReference(domainObjectIndentPropertyName);

            var objectIdentValue =
                    new CodeVariableDeclarationStatement(
                                                         this.Configuration.Environment.IdentityProperty.PropertyType,
                                                         "domainIdent",
                                                         objectIdentProperty);

            yield return objectIdentValue;

            var byIdExprByIdentityRef = this.Configuration.GetByIdExprByIdentityRef(
                                                                                    bllRefExpr,
                                                                                    objectIdentValue.ToVariableReferenceExpression());

            var domainDecl = new CodeVariableDeclarationStatement(
                                                                  this.DomainType,
                                                                  "domainObject",
                                                                  byIdExprByIdentityRef);

            yield return domainDecl;

            yield return
                    bllRefExpr.ToMethodInvokeExpression("CheckAccess", domainDecl.ToVariableReferenceExpression())
                              .ToExpressionStatement();
        }

        var result =
                evaluateDataExpr.GetContext()
                                .ToPropertyReference(this.Configuration.FileItemToMemoryStreamContextServiceName)
                                .ToMethodInvokeExpression("GetMemoryStream", fileItemDecl.ToVariableReferenceExpression())
                                .ToResultStatement(this.ReturnType);

        yield return result;
    }
}
