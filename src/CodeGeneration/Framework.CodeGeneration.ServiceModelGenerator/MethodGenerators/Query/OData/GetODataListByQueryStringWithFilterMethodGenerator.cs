using System.CodeDom;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData.Base;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData;

public class GetODataListByQueryStringWithFilterMethodGenerator<TConfiguration> : GetByODataQueryWithFilterMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public GetODataListByQueryStringWithFilterMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType, filterType)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetODataListByQueryStringWithFilter, filterType, this.DTOType);

        this.Name = this.FilterType.Name.Skip(this.DomainType.Name).SkipLast("Model").Replace("OData", "").Pipe(filterBody => this.CreateName(true, "ODataQueryStringWith" + filterBody));
    }


    public override MethodIdentity Identity { get; }

    protected override string Name { get; }


    protected override string GetComment() => $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string and filter ({this.FilterType.Name})";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.FilterType, BLL.Domain.DTO.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }
}
