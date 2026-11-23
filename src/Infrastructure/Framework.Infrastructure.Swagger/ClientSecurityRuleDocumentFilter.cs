using Anch.SecuritySystem.SecurityRuleInfo;

using System.Text.Json.Nodes;

using Microsoft.OpenApi;

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
            Type = JsonSchemaType.Integer,
            Enum = Enumerable.Range(0, ruleNames.Length).Select(JsonNode (v) => JsonValue.Create(v)).ToList(),
        };

        var apiNames = new JsonArray();
        foreach (var name in ruleNames)
        {
            apiNames.Add(name);
        }

        schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
        schema.Extensions["x-enumNames"] = new JsonNodeExtension(apiNames);

        return schema;
    }
}
