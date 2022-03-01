using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;

namespace SampleSystem.CodeGenerate.ServerDTO
{
    public class DefaultFullRefDTOFileFactory<TConfiguration> : RefDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        public DefaultFullRefDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }

        public override MainDTOFileType FileType { get; } = SampleSystemFileType.FullRefDTO;
    }
}