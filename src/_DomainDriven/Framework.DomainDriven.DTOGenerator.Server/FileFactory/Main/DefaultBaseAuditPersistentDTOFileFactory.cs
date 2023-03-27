namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultBaseAuditPersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAuditPersistentDTO;
}
