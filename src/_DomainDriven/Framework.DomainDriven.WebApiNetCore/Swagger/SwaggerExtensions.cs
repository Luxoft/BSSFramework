using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Framework.DomainDriven.WebApiNetCore.Swagger;

public static class SwaggerExtensions
{
    public static SwaggerGenOptions AddClientSecurityRule(this SwaggerGenOptions options, string clientSecurityRuleName)
    {
        options.DocumentFilter<ClientSecurityRuleDocumentFilter>(clientSecurityRuleName);

        return options;
    }
}
