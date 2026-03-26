using Framework.CodeGeneration.DTOGenerator.FileType;

using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration>(TConfiguration configuration)
    : MainDTOFileFactory<TConfiguration>(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.BaseAuditPersistentDTO;
}
