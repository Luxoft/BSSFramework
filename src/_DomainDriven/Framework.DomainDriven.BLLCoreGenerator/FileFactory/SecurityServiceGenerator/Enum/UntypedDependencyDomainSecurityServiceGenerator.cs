using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;
using Framework.QueryableSource;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLCoreGenerator
{
    public class UntypedDependencyDomainSecurityServiceGenerator<TConfiguration> : DomainSecurityServiceGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        private readonly DependencySecurityAttribute dependencySecurityAttr;

        public UntypedDependencyDomainSecurityServiceGenerator(TConfiguration configuration, Type domainType, DependencySecurityAttribute dependencySecurityAttr)
            : base(configuration, domainType)
        {
            this.dependencySecurityAttr = dependencySecurityAttr ?? throw new ArgumentNullException(nameof(dependencySecurityAttr));

            this.BaseServiceType = typeof(UntypedDependencyDomainSecurityService<,,,,>).MakeGenericType(
                    this.Configuration.Environment.PersistentDomainObjectBaseType,
                    this.DomainType,
                    this.dependencySecurityAttr.SourceType,
                    this.Configuration.Environment.GetIdentityType(),
                    this.Configuration.Environment.SecurityOperationCodeType).ToTypeReference();
        }

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
            yield return (typeof(IDomainSecurityService<,>).ToTypeReference(this.dependencySecurityAttr.SourceType, this.Configuration.Environment.SecurityOperationCodeType), "baseDomainSecurityService");
            yield return (typeof(IQueryableSource<>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType), "queryableSource");
        }
    }
}
