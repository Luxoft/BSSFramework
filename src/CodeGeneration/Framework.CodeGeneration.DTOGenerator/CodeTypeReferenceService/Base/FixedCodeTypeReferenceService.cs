using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public class FixedCodeTypeReferenceService<TConfiguration>(
    TConfiguration configuration,
    RoleFileType referenceFileType,
    RoleFileType collectionFileType,
    bool enabledSecurity = true)
    : LayerCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    private readonly RoleFileType referenceFileType = referenceFileType ?? throw new ArgumentNullException(nameof(referenceFileType));

    private readonly RoleFileType collectionFileType = collectionFileType ?? throw new ArgumentNullException(nameof(collectionFileType));

    public override bool IsOptional(PropertyInfo property) => enabledSecurity && base.IsOptional(property);

    public override RoleFileType GetReferenceFileType(PropertyInfo _) => this.referenceFileType;

    public override RoleFileType GetCollectionFileType(PropertyInfo _) => this.collectionFileType;
}

