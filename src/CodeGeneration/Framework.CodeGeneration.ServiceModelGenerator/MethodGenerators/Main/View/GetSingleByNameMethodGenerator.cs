using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetSingleByNameMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
    : ViewMethodGenerator<TConfiguration>(configuration, domainType, dtoType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = new(MethodIdentityType.GetSingleByName, dtoType);

    protected override string Name => this.CreateName(false, "Name");

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);


    protected override string GetComment() => $"Get {this.DomainType.Name} ({this.DTOType}) by name";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Name");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectDecl = this.ToDomainObjectVarDeclByName(evaluateDataExpr, bllRefExpr);

        yield return domainObjectDecl;

        yield return domainObjectDecl.ToVariableReferenceExpression()
                                     .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                     .ToMethodReturnStatement();
    }
}
