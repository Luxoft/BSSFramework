using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

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

            this.BaseServiceType = typeof(ContextDomainSecurityServiceBase<,,,>).ToTypeReference(
                this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                this.DomainTypeReference,
                this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference());
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

        public override IEnumerable<(CodeTypeReference ParameterType, string Name)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(IDisabledSecurityProviderSource).ToTypeReference(), "disabledSecurityProviderSource");
            yield return (typeof(ISecurityOperationResolver<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType, this.Configuration.Environment.SecurityOperationCodeType), "securityOperationResolver");
            yield return (typeof(IAuthorizationSystem<>).ToTypeReference(this.Configuration.Environment.GetIdentityType()), "authorizationSystem");
            yield return (typeof(ISecurityExpressionBuilderFactory<,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType, this.Configuration.Environment.GetIdentityType()), "securityExpressionBuilderFactory");
        }
    }
}
