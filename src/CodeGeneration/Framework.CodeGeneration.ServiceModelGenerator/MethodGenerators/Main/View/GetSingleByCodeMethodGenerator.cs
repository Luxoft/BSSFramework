using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetSingleByCodeMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
    : ViewMethodGenerator<TConfiguration>(configuration, domainType, dtoType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = new(MethodIdentityType.GetSingleByCode, dtoType);

    protected override string Name => this.CreateName(false, "Code");

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);


    protected override string GetComment() => $"Get {this.DomainType.Name} ({this.DTOType}) by code";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.DomainType.GetInterfaceImplementationArgument(typeof(ICodeObject<>)).ToTypeReference().ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Code");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectDecl = this.ToDomainObjectVarDeclByCode(evaluateDataExpr, bllRefExpr);

        yield return domainObjectDecl;

        yield return domainObjectDecl.ToVariableReferenceExpression()
                                     .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                     .ToMethodReturnStatement();
    }
}
