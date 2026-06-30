using System.Reflection;

using Anch.Core;

using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.FileGeneration.Configuration;
using Framework.Relations;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class DynamicCodeTypeReferenceService<TConfiguration>(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType detailFileType)
    : LayerCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    private readonly RoleFileType referenceFileType = referenceFileType ?? throw new ArgumentNullException(nameof(referenceFileType));

    private readonly RoleFileType detailFileType = detailFileType ?? throw new ArgumentNullException(nameof(detailFileType));

    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        return property.IsDetail() ? this.detailFileType : this.referenceFileType;
    }


    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        if (property.IsDetail())
        {
            return this.detailFileType;
        }

        if (property.IsNotDetail())
        {
            return this.referenceFileType;
        }

        if (!this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(property.PropertyType.GetCollectionElementType()))
        {
            if (!property.IsDetail())
            {
                return this.referenceFileType;
            }
        }

        return this.detailFileType;
    }
}

