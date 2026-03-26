using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;
using Framework.CodeGeneration.DTOGenerator.FileType;

using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.CodeTypeReferenceService;

public class CryptCodeTypeReferenceService<TConfiguration>(
    TConfiguration configuration,
    RoleFileType referenceFileType,
    RoleFileType collectionFileType)
    : DynamicCodeTypeReferenceService<TConfiguration>(configuration, referenceFileType, collectionFileType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override bool IsOptional(PropertyInfo property)
    {
        return false;
    }
}
