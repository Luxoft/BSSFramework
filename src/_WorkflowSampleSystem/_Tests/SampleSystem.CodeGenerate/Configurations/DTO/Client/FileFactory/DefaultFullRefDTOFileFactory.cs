using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;

namespace SampleSystem.CodeGenerate.ClientDTO
{
    public class DefaultFullRefDTOFileFactory<TConfiguration> : RefDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultFullRefDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOFileType FileType { get; } = SampleSystemFileType.FullRefDTO;
    }
}