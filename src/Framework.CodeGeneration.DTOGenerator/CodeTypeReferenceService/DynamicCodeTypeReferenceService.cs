using System.Reflection;

using CommonFramework;

using Framework.Application.Domain.Attributes;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class DynamicCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly RoleFileType referenceFileType;

    private readonly RoleFileType detailFileType;


    public DynamicCodeTypeReferenceService(TConfiguration configuration, RoleFileType referenceFileType, RoleFileType detailFileType)
            : base(configuration)
    {
        if (referenceFileType == null) throw new ArgumentNullException(nameof(referenceFileType));
        if (detailFileType == null) throw new ArgumentNullException(nameof(detailFileType));

        this.referenceFileType = referenceFileType;
        this.detailFileType = detailFileType;
    }


    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

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
