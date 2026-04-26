using System.CodeDom;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.Projection;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetSingleByIdentityMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
    : ViewMethodGenerator<TConfiguration>(configuration, domainType, dtoType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = new(MethodIdentityType.GetSingleByIdentity, dtoType);

    protected override string Name => this.CreateName(false, null);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);


    protected override string GetComment() => $"Get {this.DomainType.Name} ({this.DTOType}) by identity";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType.GetProjectionSourceTypeOrSelf(), BLL.Domain.DTO.DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectDecl = this.ToDomainObjectVarDeclById(evaluateDataExpr, bllRefExpr, true);

        yield return domainObjectDecl;

        yield return domainObjectDecl.ToVariableReferenceExpression()
                                     .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                     .ToMethodReturnStatement();
    }
}
