using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;

namespace SampleSystem.CodeGenerate.ClientDTO
{
    public class DefaultSimpleRefFullDetailDTOFileFactory<TConfiguration> : RefDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultSimpleRefFullDetailDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOFileType FileType { get; } = SampleSystemFileType.SimpleRefFullDetailDTO;
    }
}