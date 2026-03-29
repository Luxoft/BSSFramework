using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.FileStore;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.FileStore;

public class GetFileContainerMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, Type targetDomainType)
    : FileStoreMethodGeneratorBase<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.GetFileContainer;


    protected override string Name => $"GetRich{targetDomainType.Name}FileContainer";

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.ObjectFileContainerType, DTOType.RichDTO);


    protected override string GetComment() => $"Get {this.DomainType.Name} attachments";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return new CodeParameterDeclarationExpression(this.Configuration.Environment.GetIdentityType(), "objectIdentity");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var accessMethod in this.GetCheckAccessMethods(evaluateDataExpr, targetDomainType, this.Parameter.ToVariableReferenceExpression()))
        {
            yield return accessMethod;
        }

        var objectFileContainerDecl = this.GetDefaultBLLVariableDeclaration(evaluateDataExpr, "objectFileContainerBLL", this.Configuration.ObjectFileContainerType);

        yield return objectFileContainerDecl;

        var getForObjectMethod = objectFileContainerDecl.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.GetForObjectMethodName, this.Parameter.ToVariableReferenceExpression());

        var resultDecl = this.Configuration.ObjectFileContainerType.ToTypeReference().ToVariableDeclarationStatement("result");

        yield return resultDecl;

        yield return getForObjectMethod.ToAssignStatement(resultDecl.ToVariableReferenceExpression());

        yield return this.ReturnType.ToObjectCreateExpression(evaluateDataExpr.GetMappingService(), resultDecl.ToVariableReferenceExpression()).ToMethodReturnStatement();
    }
}
