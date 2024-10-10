using Framework.SecuritySystem.SecurityRuleInfo;

using Microsoft.Extensions.DependencyInjection;
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
                         Type = "integer"
                     };

        var apiValues = new OpenApiArray();
        apiValues.AddRange(Enumerable.Range(0, ruleNames.Length).Select(v => new OpenApiInteger(v)).ToArray());
        schema.Extensions["enum"] = apiValues;

        var apiNames = new OpenApiArray();
        apiNames.AddRange(ruleNames.Select(v => new OpenApiString(v)));
        schema.Extensions["x-enumNames"] = apiNames;

        context.SchemaRepository.Schemas.Add(clientSecurityRuleName, schema);
    }
}

public static class SwaggerExtensions
{
    public static SwaggerGenOptions AddClientSecurityRule(this SwaggerGenOptions options, string clientSecurityRuleName)
    {
        options.DocumentFilter<ClientSecurityRuleDocumentFilter>(clientSecurityRuleName);

        return options;
    }
}
