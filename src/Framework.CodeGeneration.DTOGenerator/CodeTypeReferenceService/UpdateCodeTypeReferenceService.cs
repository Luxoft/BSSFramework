using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.Application.Domain.Attributes;
using Framework.BLL.Domain.MergeItemData;
using Framework.CodeDom;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class UpdateCodeTypeReferenceService<TConfiguration> : LayerCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public UpdateCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public override bool IsOptional(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var isIdOrVersion = this.Configuration.IsIdentityOrVersionProperty(property);

        var isCollection = property.PropertyType.GetCollectionElementType().Maybe(this.IsDomainType);

        return !isIdOrVersion && !isCollection;
    }

    public override RoleFileType GetReferenceFileType(PropertyInfo property)
    {
        return !property.IsDetail() && this.Configuration.IsPersistentObject(property.PropertyType)
                       ? FileType.FileType.IdentityDTO
                       : FileType.FileType.UpdateDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? FileType.FileType.IdentityDTO
                       : FileType.FileType.UpdateDTO;
    }

    protected override CodeTypeReference GetCollectionCodeTypeReference(Type elementType, FileType.FileType elementFileType)
    {
        var elementTypeRef = this.Configuration.GetCodeTypeReference(elementType, elementFileType);

        var identityTypeRef = this.Configuration.GetCodeTypeReference(elementType, FileType.FileType.IdentityDTO);

        return typeof(UpdateItemData<,>).ToTypeReference(elementTypeRef, identityTypeRef).ToCollectionReference(this.Configuration.CollectionType);
    }
}
