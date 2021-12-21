using System;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class DefaultFullDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public DefaultFullDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }

        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.FullDTO;
    }
}