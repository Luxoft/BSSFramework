using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator
{
    public interface ILayerCodeTypeReferenceService : IPropertyCodeTypeReferenceService
    {
        Type CollectionType { get; }

        RoleFileType GetReferenceFileType(PropertyInfo property);

        RoleFileType GetCollectionFileType(PropertyInfo property);
    }

    public abstract class LayerCodeTypeReferenceService<TConfiguration> : PropertyCodeTypeReferenceService<TConfiguration>, ILayerCodeTypeReferenceService
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        protected LayerCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
        {
        }


        public virtual Type CollectionType => this.Configuration.CollectionType;


        public abstract RoleFileType GetReferenceFileType(PropertyInfo property);

        public abstract RoleFileType GetCollectionFileType(PropertyInfo property);


        public override RoleFileType GetFileType(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var elementType = property.PropertyType.GetCollectionElementType();

            if (elementType != null && this.IsDomainType(elementType))
            {
                return this.GetCollectionFileType(property);
            }
            else if (this.IsDomainType(property.PropertyType))
            {
                return this.GetReferenceFileType(property);
            }
            else
            {
                return this.Configuration.DefaultCodeTypeReferenceService.GetFileType(property);
            }
        }

        protected virtual CodeTypeReference GetCollectionCodeTypeReference(Type elementType, FileType elementFileType)
        {
            return this.Configuration.GetCodeTypeReference(elementType, elementFileType).ToCollectionReference(this.CollectionType);
        }


        protected override CodeTypeReference GetCodeTypeReferenceByProperty(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var elementType = property.PropertyType.GetCollectionElementType();

            if (elementType != null && this.IsDomainType(elementType))
            {
                return this.GetCollectionCodeTypeReference(elementType, this.GetCollectionFileType(property));
            }
            else if (this.IsDomainType(property.PropertyType))
            {
                return this.Configuration.GetCodeTypeReference(property.PropertyType, this.GetReferenceFileType(property));
            }
            else
            {
                return base.GetCodeTypeReferenceByProperty(property);
            }
        }
    }
}
