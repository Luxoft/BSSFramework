using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.DomainDriven.ServiceModelGenerator;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiGenerator.NetCore;

public class WebApiNetCoreFileFactory<TConfiguration> : WebApiNetCoreFileFactoryBase<TConfiguration>
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly List<CodeAttributeDeclaration> additionalAttributes;

    public WebApiNetCoreFileFactory(
        TConfiguration configuration,
        Type domainType,
        List<CodeAttributeDeclaration> additionalAttributes)
        : base(configuration, domainType) =>
        this.additionalAttributes = additionalAttributes ?? new List<CodeAttributeDeclaration>();

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        return this.additionalAttributes.Union(
            this.GetDefaultAttributes(),
            new EqualityComparerImpl<CodeAttributeDeclaration>(
                (a1, a2) => CodeTypeReferenceComparer.Value.Equals(a1.AttributeType, a2.AttributeType)));
    }

    private IEnumerable<CodeAttributeDeclaration> GetDefaultAttributes()
    {
        yield return new CodeAttributeDeclaration(typeof(ApiControllerAttribute).ToTypeReference());

        var routeTemplate = this.Configuration.UseRouteAction ? "api/[controller]/[action]" : "api/[controller]";

        yield return new CodeAttributeDeclaration(
            typeof(RouteAttribute).ToTypeReference(),
            new CodeAttributeArgument(new CodePrimitiveExpression(routeTemplate)));

        foreach (var attribute in base.GetCustomAttributes().Concat(this.additionalAttributes))
        {
            yield return attribute;
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers() =>
        this.GetMethodGenerators()
            .SelectMany(z => z.GetFacadeMethods())
            .Select(z => z.Attributes.HasFlag(MemberAttributes.Public) ?
                              this.Configuration.UseRouteAction
                                     ? z.WithAttributeMethods<HttpPostAttribute>()
                                     : z.WithAttributeMethods<HttpPostAttribute>(new CodeAttributeArgument(new CodePrimitiveExpression(z.Name)))
                             : z)
            .Select(
                z => (z.Attributes.HasFlag(MemberAttributes.Public) && z.Parameters.Count < 2)
                         ? z.WithAttributeParameter<FromBodyAttribute>()
                         : z)
            .Concat(base.GetMembers())
            .OrderBy(z => z.Name);
}
