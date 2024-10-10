using Framework.SecuritySystem.SecurityRuleInfo;

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Framework.DomainDriven.WebApiNetCore.Swagger;

public class ClientSecurityRuleDocumentFilter(IClientSecurityRuleInfoSource source, string clientSecurityRuleName) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var ruleNames = source.GetInfos().Select(info => info.Header.Name).OrderBy(v => v).ToArray();

        var schema = new OpenApiSchema
                     {
                         Type = "integer",
                         Enum = Enumerable.Range(0, ruleNames.Length).Select(v => (IOpenApiAny)new OpenApiInteger(v)).ToList(),
                     };

        var apiNames = new OpenApiArray();
        apiNames.AddRange(ruleNames.Select(v => new OpenApiString(v)));
        schema.Extensions["x-enumNames"] = apiNames;

        context.SchemaRepository.Schemas.Add(clientSecurityRuleName, schema);
    }
}
