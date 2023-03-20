using System;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultRichEventDTOFileFactory<TConfiguration> : EventDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultRichEventDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override DTOFileType FileType { get; } = ServerFileType.RichEventDTO;
}
