using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using SecuritySystem.SecurityRuleInfo;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Framework.Infrastructure.Swagger;

public class ClientSecurityRuleDocumentFilter(IClientSecurityRuleInfoSource source, string clientSecurityRuleName) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) =>
        context.SchemaRepository.Schemas.Add(clientSecurityRuleName, this.BuildSchema());

    private OpenApiSchema BuildSchema()
    {
        var ruleNames = source.GetInfos().Select(info => info.Rule.Name).OrderBy(v => v).ToArray();

        var schema = new OpenApiSchema
                     {
                         Type = "integer",
                         Enum = Enumerable.Range(0, ruleNames.Length).Select(IOpenApiAny (v) => new OpenApiInteger(v)).ToList(),
                     };

        var apiNames = new OpenApiArray();
        apiNames.AddRange(ruleNames.Select(v => new OpenApiString(v)));
        schema.Extensions["x-enumNames"] = apiNames;

        return schema;
    }
}
