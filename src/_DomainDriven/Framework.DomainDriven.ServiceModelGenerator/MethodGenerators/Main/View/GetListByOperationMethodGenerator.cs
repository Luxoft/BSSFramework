using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetListByOperationMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetListByOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetListByOperation, this.DTOType);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name => this.CreateName(true, "Operation");


    protected override string GetComment()
    {
        return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.GetSecurityRuleParameter();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        return  this.GetSecurityRuleParameter().ToVariableReferenceExpression();
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return bllRefExpr.ToMethodInvokeExpression("GetFullList", this.GetFetchsExpression(evaluateDataExpr))
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();
    }
}
