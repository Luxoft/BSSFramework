using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.BLL.DTOMapping.MergeItemData;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.Relations;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class UpdateCodeTypeReferenceService<TConfiguration>(TConfiguration configuration) : LayerCodeTypeReferenceService<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
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
                       ? BaseFileType.IdentityDTO
                       : BaseFileType.UpdateDTO;
    }

    public override RoleFileType GetCollectionFileType(PropertyInfo property)
    {
        var elementType = property.PropertyType.GetCollectionElementType();

        return !property.IsDetail() && !this.DomainTypeIsPersistent(property) && this.Configuration.IsPersistentObject(elementType)
                       ? BaseFileType.IdentityDTO
                       : BaseFileType.UpdateDTO;
    }

    protected override CodeTypeReference GetCollectionCodeTypeReference(Type elementType, BaseFileType elementFileType)
    {
        var elementTypeRef = this.Configuration.GetCodeTypeReference(elementType, elementFileType);

        var identityTypeRef = this.Configuration.GetCodeTypeReference(elementType, BaseFileType.IdentityDTO);

        return typeof(UpdateItemData<,>).ToTypeReference(elementTypeRef, identityTypeRef).ToCollectionReference(this.Configuration.CollectionType);
    }
}
