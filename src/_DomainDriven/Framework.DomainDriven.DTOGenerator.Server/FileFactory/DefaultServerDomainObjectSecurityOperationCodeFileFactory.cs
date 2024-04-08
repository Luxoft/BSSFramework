using System.CodeDom;

using Framework.SecuritySystem;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultServerDomainObjectSecurityOperationCodeFileFactory<TConfiguration> : DomainObjectSecurityOperationCodeFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultServerDomainObjectSecurityOperationCodeFileFactory(TConfiguration configuration, Type domainType, IEnumerable<SecurityRule> securityOperations)
            : base(configuration, domainType, securityOperations)
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
