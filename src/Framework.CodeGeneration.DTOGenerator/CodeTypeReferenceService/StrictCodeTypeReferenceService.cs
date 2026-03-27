using System.Reflection;

using CommonFramework;

using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.Database.Attributes;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class StrictCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : LayerCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override bool IsOptional(PropertyInfo property)
    {
        return !this.Configuration.ExpandStrictMaybeToDefault && base.IsOptional(property);
    }

    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        return !property.IsDetail() && this.Configuration.IsPersistentObject(property.PropertyType)
                       ? BaseFileType.IdentityDTO
                       : BaseFileType.StrictDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? BaseFileType.IdentityDTO
                       : BaseFileType.StrictDTO;
    }
}
