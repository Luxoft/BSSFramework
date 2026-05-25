using System.CodeDom;
using System.Reflection;

using Anch.Core;

using Framework.BLL.Domain.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

public interface IPropertyCodeTypeReferenceService : ICodeTypeReferenceService
{
    bool IsOptional(PropertyInfo property);

    bool IsCollection(PropertyInfo property);

    CodeTypeReference GetCodeTypeReference(PropertyInfo property, bool withOptional = false);
}

public class PropertyCodeTypeReferenceService<TConfiguration>(TConfiguration configuration)
    : CodeTypeReferenceService<TConfiguration>(configuration), IPropertyCodeTypeReferenceService
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    public virtual bool IsOptional(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.Configuration.Environment.MetadataProxyProvider.Wrap(property).IsSecurity();
    }

    public virtual bool IsCollection(PropertyInfo property) => property.PropertyType.GetCollectionElementType().Maybe(this.IsDomainType);

    public CodeTypeReference GetCodeTypeReference(PropertyInfo property, bool withOptional = false)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var typeRef = this.GetCodeTypeReferenceByProperty(property);

        return withOptional && this.IsOptional(property) ? typeRef.ToMaybeReference() : typeRef;
    }

    protected virtual bool IsDomainType(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this.Configuration.DomainTypes.Contains(type);
    }

    protected bool DomainTypeIsPersistent(PropertyInfo propertyInfo) => this.Configuration.IsPersistentObject(propertyInfo.ReflectedType);

    protected virtual CodeTypeReference GetCodeTypeReferenceByProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.GetCodeTypeReferenceByType(property.PropertyType);
    }
}
