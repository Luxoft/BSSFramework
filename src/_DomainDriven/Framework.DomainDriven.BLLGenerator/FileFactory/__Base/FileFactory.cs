using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public abstract class FileFactory<TConfiguration>(TConfiguration configuration, Type? domainType) :
    CodeFileFactory<TConfiguration, FileType>(configuration, domainType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>;
