using System.Reflection;

using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public class FixedCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    private readonly RoleFileType referenceFileType;

    private readonly RoleFileType collectionFileType;

    private readonly bool enabledSecurity;


    public FixedCodeTypeReferenceService(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType collectionFileType, bool enabledSecurity = true)
            : base(configuration)
    {
        if (referenceFileType == null) throw new ArgumentNullException(nameof(referenceFileType));
        if (collectionFileType == null) throw new ArgumentNullException(nameof(collectionFileType));

        this.referenceFileType = referenceFileType;
        this.collectionFileType = collectionFileType;
        this.enabledSecurity = enabledSecurity;
    }


    public override bool IsOptional(PropertyInfo property) => this.enabledSecurity && base.IsOptional(property);

    public override RoleFileType GetReferenceFileType(PropertyInfo _) => this.referenceFileType;

    public override RoleFileType GetCollectionFileType(PropertyInfo _) => this.collectionFileType;
}
