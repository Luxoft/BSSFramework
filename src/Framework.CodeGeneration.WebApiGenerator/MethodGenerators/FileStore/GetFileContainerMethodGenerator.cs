using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Configuration.FileStore;
using Framework.CodeGeneration.WebApiGenerator.Extensions;


namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.FileStore;

public class GetFileContainerMethodGenerator<TConfiguration> : FileStoreMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IFileStoreGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type targetDomainType;


    public GetFileContainerMethodGenerator(TConfiguration configuration, Type domainType, Type targetDomainType)
            : base(configuration, domainType)
    {
        this.targetDomainType = targetDomainType;
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.GetFileContainer;


    protected override string Name => $"GetRich{this.targetDomainType.Name}FileContainer";

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.Configuration.ObjectFileContainerType, DTOType.RichDTO);


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} attachments";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return new CodeParameterDeclarationExpression(this.Configuration.Environment.GetIdentityType(), "objectIdentity");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var accessMethod in this.GetCheckAccessMethods(evaluateDataExpr, this.targetDomainType, this.Parameter.ToVariableReferenceExpression()))
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
