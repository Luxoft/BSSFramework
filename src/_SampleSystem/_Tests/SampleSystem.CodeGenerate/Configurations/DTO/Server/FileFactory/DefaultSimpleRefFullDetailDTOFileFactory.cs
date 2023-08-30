using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class DefaultSimpleRefFullDetailDTOFileFactory<TConfiguration> : RefDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultSimpleRefFullDetailDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MainDTOFileType FileType { get; } = SampleSystemFileType.SimpleRefFullDetailDTO;
}
