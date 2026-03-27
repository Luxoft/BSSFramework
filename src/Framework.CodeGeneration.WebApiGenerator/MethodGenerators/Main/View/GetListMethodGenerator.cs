using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Configuration.Main;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View._Base;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View;

public class GetListMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IMainGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetListMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetList, this.DTOType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, null);

    protected override string GetComment()
    {
        return $"Get full list of {this.DomainType.GetPluralizedDomainName()} ({this.DTOType})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield break;
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return bllRefExpr.ToMethodInvokeExpression("GetFullList", this.GetFetchRule())
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();
    }
}
