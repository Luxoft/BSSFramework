using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.OData;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetODataListByQueryStringWithFilterMethodGenerator<TConfiguration> : GetByODataQueryWithFilterMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetODataListByQueryStringWithFilterMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType, filterType)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetODataListByQueryStringWithFilter, filterType, this.DTOType);

        this.Name = this.FilterType.Name.Skip(this.DomainType.Name).SkipLast("Model").Replace("OData", "").Pipe(filterBody => this.CreateName(true, "ODataQueryStringWith" + filterBody));
    }


    public override MethodIdentity Identity { get; }

    protected override string Name { get; }


    protected override string GetComment()
    {
        return $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string and filter ({this.FilterType.Name})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.FilterType, Transfering.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }


    protected override CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr)
    {
        return evaluateDataExpr.GetContext().ToPropertyReference((IODataBLLContext c) => c.SelectOperationParser)
                               .ToMethodInvokeExpression((IParser<string, SelectOperation> parser) => parser.Parse(null), this.Parameter.ToVariableReferenceExpression());
    }
}
