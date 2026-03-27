using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View._Base;
using Framework.Core;
using Framework.Projection;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View;

public class GetListByIdentsMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetListByIdentsMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetListByIdents, this.DTOType);
    }

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, "Idents");


    protected override string GetComment()
    {
        return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by idents";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType.GetProjectionSourceTypeOrSelf(), BLL.Domain.DTO.DTOType.IdentityDTO)
                         .ToArrayReference()
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Idents");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return bllRefExpr.ToMethodInvokeExpression("GetListByIdents", this.Parameter.ToVariableReferenceExpression(), this.GetFetchRule())
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();
    }
}
