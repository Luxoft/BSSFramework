using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetListByFilterModelMethodGenerator<TConfiguration> : ViewCollectionMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type _filterType;


    public GetListByFilterModelMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType)
    {
        if (filterType == null) throw new ArgumentNullException(nameof(filterType));

        this._filterType = filterType;

        this.Identity = new MethodIdentity(MethodIdentityType.GetListByFilter, this._filterType, this.DTOType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, this._filterType.Name.Skip(this.DomainType.Name).SkipLast("Model"));


    protected override string GetComment()
    {
        return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by filter ({this._filterType})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this._filterType, Transfering.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var typedFilterDecl = this._filterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter",
            this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return typedFilterDecl;

        yield return bllRefExpr.ToMethodInvokeExpression(nameof(IBLLQueryBase<object>.GetListBy), typedFilterDecl.ToVariableReferenceExpression(), this.GetFetchsExpression(evaluateDataExpr))
                               .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();

    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this._filterType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityOperation;
    }
}
