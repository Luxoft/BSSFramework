using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public interface IDTOSource : IDomainTypeContainer//, IFileTypeSource<DTOFileType>
{
    DTOFileType FileType { get; }
}

public interface IDTOSource<out TConfiguration> : IDTOSource, IGeneratorConfigurationContainer<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{

}
