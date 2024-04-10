using System.CodeDom;

using Framework.SecuritySystem;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultServerDomainObjectSecurityRuleCodeFileFactory<TConfiguration> : DomainObjectSecurityRuleCodeFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultServerDomainObjectSecurityRuleCodeFileFactory(TConfiguration configuration, Type domainType, IEnumerable<SecurityRule> securityRules)
            : base(configuration, domainType, securityRules)
    {
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
    }
}
