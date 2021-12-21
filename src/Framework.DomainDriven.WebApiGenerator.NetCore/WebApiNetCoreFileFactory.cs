using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.ServiceModelGenerator;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiGenerator.NetCore
{
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
            return this.additionalAttributes.Union(this.GetDefaultAttributes(), new EqualityComparerImpl<CodeAttributeDeclaration>((a1, a2) => CodeTypeReferenceComparer.Value.Equals(a1.AttributeType, a2.AttributeType)));
        }

        private IEnumerable<CodeAttributeDeclaration> GetDefaultAttributes()
        {
            yield return new CodeAttributeDeclaration(typeof(ApiControllerAttribute).ToTypeReference());

            yield return new CodeAttributeDeclaration(
                typeof(ApiVersionAttribute).ToTypeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression("1.0")));

            yield return new CodeAttributeDeclaration(
                typeof(RouteAttribute).ToTypeReference(),
                new CodeAttributeArgument(new CodePrimitiveExpression("api/v{version:apiVersion}/[controller]")));

            foreach (var attribute in base.GetCustomAttributes().Concat(this.additionalAttributes))
            {
                yield return attribute;
            }
        }

        protected override IEnumerable<CodeTypeMember> GetMembers() =>
            this.GetMethodGenerators()
                .SelectMany(z => z.GetFacadeMethods())
                .Select(z => z.Attributes.HasFlag(MemberAttributes.Public) ? z.WithAttributeMethods<HttpPostAttribute>() : z)
                .Select(
                    z => z.Attributes.HasFlag(MemberAttributes.Public)
                             ? z.WithAttributeMethods<RouteAttribute>(new CodeAttributeArgument(new CodePrimitiveExpression(z.Name)))
                             : z)
                .Select(
                    z => (z.Attributes.HasFlag(MemberAttributes.Public) && z.Parameters.Count < 2)
                             ? z.WithAttributeParameter<FromBodyAttribute>()
                             : z)
                .Concat(base.GetMembers())
                .OrderBy(z => z.Name);
    }
}
