using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;

public class GetListByOperationMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetListByOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType) =>
        this.Identity = new MethodIdentity(MethodIdentityType.GetListByOperation, this.DTOType);

    public override MethodIdentity Identity { get; }


    protected override string Name => this.CreateName(true, "Operation");


    protected override string GetComment() => $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.GetSecurityRuleParameter();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr) => this.GetSecurityRuleParameter().ToVariableReferenceExpression();

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return bllRefExpr.ToMethodInvokeExpression("GetFullList", this.GetFetchRule())
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();
    }
}
