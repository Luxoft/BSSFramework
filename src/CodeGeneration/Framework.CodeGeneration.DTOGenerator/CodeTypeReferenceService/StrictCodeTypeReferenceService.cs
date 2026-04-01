using System.Reflection;

using CommonFramework;

using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.FileGeneration.Configuration;
using Framework.Relations;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class StrictCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : LayerCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    public override bool IsOptional(PropertyInfo property) => !this.Configuration.ExpandStrictMaybeToDefault && base.IsOptional(property);

    public override RoleFileType GetReferenceFileType(PropertyInfo property) =>
        !property.IsDetail() && this.Configuration.IsPersistentObject(property.PropertyType)
            ? BaseFileType.IdentityDTO
            : BaseFileType.StrictDTO;

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? BaseFileType.IdentityDTO
                       : BaseFileType.StrictDTO;
    }
}
