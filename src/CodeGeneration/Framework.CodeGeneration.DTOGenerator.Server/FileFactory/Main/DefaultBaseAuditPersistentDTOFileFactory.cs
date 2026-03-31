using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(TConfiguration configuration)
    : MainDTOFileFactory<TConfiguration>(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.BaseAuditPersistentDTO;
}
