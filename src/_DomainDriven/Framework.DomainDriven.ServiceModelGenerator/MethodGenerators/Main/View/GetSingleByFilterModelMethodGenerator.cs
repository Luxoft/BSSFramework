using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetSingleByFilterModelMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type filterType;


    public GetSingleByFilterModelMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType)
    {
        this.filterType = filterType ?? throw new ArgumentNullException(nameof(filterType));

        this.Identity = new MethodIdentity(MethodIdentityType.GetSingleByFilter, this.filterType, this.DTOType);
    }

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(false, this.filterType.Name.Skip(this.DomainType.Name).SkipLast("Model"));

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);

    protected override string GetComment()
    {
        return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by filter ({this.filterType})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.filterType, Transfering.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var typedFilterDeсl = this.filterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return typedFilterDeсl;

        yield return bllRefExpr.ToMethodInvokeExpression("GetObjectBy", typedFilterDeсl.ToVariableReferenceExpression(), this.GetFetchsExpression(evaluateDataExpr))
                               .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                               .ToMethodReturnStatement();

    }

    protected override object GetBLLSecurityParameter()
    {
        var modelSecurityAttribute = this.filterType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter();
        }

        return modelSecurityAttribute.SecurityOperation;
    }
}
