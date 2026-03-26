using System.Reflection;

using CommonFramework;

using Framework.Application.Domain.Attributes;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class StrictCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public StrictCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override bool IsOptional(PropertyInfo property)
    {
        return !this.Configuration.ExpandStrictMaybeToDefault && base.IsOptional(property);
    }

    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        return !property.IsDetail() && this.Configuration.IsPersistentObject(property.PropertyType)
                       ? FileType.FileType.IdentityDTO
                       : FileType.FileType.StrictDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? FileType.FileType.IdentityDTO
                       : FileType.FileType.StrictDTO;
    }
}
