using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator;

public class UpdateCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public UpdateCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override bool IsOptional(System.Reflection.PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var isIdOrVersion = this.Configuration.IsIdentityOrVersionProperty(property);

        var isCollection = property.PropertyType.GetCollectionElementType().Maybe(this.IsDomainType);

        return !isIdOrVersion && !isCollection;
    }

    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        return !property.IsDetail() && this.Configuration.IsPersistentObject(property.PropertyType)
                       ? FileType.IdentityDTO
                       : FileType.UpdateDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? FileType.IdentityDTO
                       : FileType.UpdateDTO;
    }

    protected override CodeTypeReference GetCollectionCodeTypeReference(Type elementType, FileType elementFileType)
    {
        var elementTypeRef = this.Configuration.GetCodeTypeReference(elementType, elementFileType);

        var identityTypeRef = this.Configuration.GetCodeTypeReference(elementType, FileType.IdentityDTO);

        return typeof(UpdateItemData<,>).ToTypeReference(elementTypeRef, identityTypeRef).ToCollectionReference(this.Configuration.CollectionType);
    }
}
