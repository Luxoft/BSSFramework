using System;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultSimpleEventDTOFileFactory<TConfiguration> : EventDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultSimpleEventDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override DTOFileType FileType { get; } = ServerFileType.SimpleEventDTO;
}
