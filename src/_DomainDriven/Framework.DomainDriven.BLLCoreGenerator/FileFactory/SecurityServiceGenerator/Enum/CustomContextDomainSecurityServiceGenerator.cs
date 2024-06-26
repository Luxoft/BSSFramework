﻿using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class CustomContextDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public CustomContextDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            var genericTypes = this.Configuration.GetDomainTypeSecurityParameters(this.DomainType).Select(p => p.ToTypeReference()).ToArray();

            this.DomainTypeReference = genericTypes.FirstOr(() => this.DomainType.ToTypeReference());

            this.BaseServiceType = typeof(ContextDomainSecurityServiceBase<>).ToTypeReference(
                this.DomainTypeReference);
        }


        public sealed override CodeTypeReference DomainTypeReference { get; }


        public override CodeTypeReference BaseServiceType { get; }


        public override IEnumerable<CodeTypeMember> GetMembers()
        {
            yield break;
        }


        public override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            yield break;
        }

        public override IEnumerable<(CodeTypeReference ParameterType, string Name, CodeExpression CustomBaseInvoke)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(ISecurityProvider<>).ToTypeReference(this.DomainType), "disabledSecurityProvider", null);
            yield return (typeof(ISecurityRuleExpander).ToTypeReference(), "securityRuleResolver", null);
            yield return (typeof(ISecurityPathProviderFactory).ToTypeReference(), "securityPathProviderFactory", null);
        }
    }
}
