using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator;

public interface IPropertyCodeTypeReferenceService : ICodeTypeReferenceService
{
    bool IsOptional(PropertyInfo property);

    bool IsCollection(PropertyInfo property);

    CodeTypeReference GetCodeTypeReference(PropertyInfo property, bool withOptional = false);
}

public class PropertyCodeTypeReferenceService<TConfiguration> : CodeTypeReferenceService<TConfiguration>, IPropertyCodeTypeReferenceService
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public PropertyCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
    {
    }


    public virtual bool IsOptional(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.Configuration.Environment.ExtendedMetadata.GetProperty(property).IsSecurity();
    }

    public virtual bool IsCollection(PropertyInfo property)
    {
        return property.PropertyType.GetCollectionElementType().Maybe(this.IsDomainType);
    }

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

    protected bool DomainTypeIsPersistent(PropertyInfo propertyInfo)
    {
        return this.Configuration.IsPersistentObject(propertyInfo.ReflectedType);
    }

    protected virtual CodeTypeReference GetCodeTypeReferenceByProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.GetCodeTypeReferenceByType(property.PropertyType);
    }
}
