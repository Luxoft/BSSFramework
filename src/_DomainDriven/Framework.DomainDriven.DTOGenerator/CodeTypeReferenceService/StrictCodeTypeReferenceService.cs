using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator;

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
                       ? FileType.IdentityDTO
                       : FileType.StrictDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? FileType.IdentityDTO
                       : FileType.StrictDTO;
    }
}
