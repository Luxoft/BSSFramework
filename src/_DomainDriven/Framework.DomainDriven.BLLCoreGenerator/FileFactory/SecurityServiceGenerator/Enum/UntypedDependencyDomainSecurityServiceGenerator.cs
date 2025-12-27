using System.CodeDom;

using CommonFramework.GenericRepository;

using Framework.CodeDom;
using Framework.Security;

using SecuritySystem.DomainServices;
using SecuritySystem.DomainServices.DependencySecurity;
using SecuritySystem.Expanders;

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

            this.BaseServiceType = typeof(UntypedDependencyDomainSecurityService<,>).MakeGenericType(
                    this.DomainType,
                    this.dependencySecurityAttr.SourceType).ToTypeReference();
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

        public override IEnumerable<(CodeTypeReference? ParameterType, string? Name, CodeExpression? CustomBaseInvoke)> GetBaseTypeConstructorParameters()
        {
            yield return (typeof(ISecurityRuleExpander).ToTypeReference(), "securityRuleExpander", null);
            yield return (typeof(IDomainSecurityService<>).ToTypeReference(this.dependencySecurityAttr.SourceType), "baseDomainSecurityService", null);
            yield return (typeof(IQueryableSource).ToTypeReference(), "queryableSource", null);
        }
    }
}
