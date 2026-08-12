using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData;

public class GetODataListByQueryStringMethodGenerator<TConfiguration> : GetByODataQueryMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public GetODataListByQueryStringMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType) =>
        this.Identity = new MethodIdentity(MethodIdentityType.GetODataListByQueryString, this.DTOType);

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, "ODataQueryString");


    protected override string GetComment() => $"Get {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");
    }
}
