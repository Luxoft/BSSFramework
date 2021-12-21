using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class ClientConfigurationCodeTypeReferenceService<TConfiguration> : ConfigurationCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public ClientConfigurationCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
        {
        }


        public override CodeTypeReference GetCodeTypeReferenceByType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var genericNullableType = type.GetNullableElementType();

            if (genericNullableType != null)
            {
                return this.GetCodeTypeReferenceByType(genericNullableType).ToNullableReference();
            }
            else if (type.IsEnum)
            {
                return this.Configuration.GetCodeTypeReference(type, ClientFileType.Enum);
            }
            else if (!type.IsPrimitiveType())
            {
                if (type.IsClass)
                {
                    return this.Configuration.GetCodeTypeReference(type, ClientFileType.Class);
                }
                else if (type.IsValueType)
                {
                    return this.Configuration.GetCodeTypeReference(type, ClientFileType.Struct);
                }
            }

            return base.GetCodeTypeReferenceByType(type);
        }

        public override RoleFileType GetFileType(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var type = property.PropertyType.GetCollectionOrArrayElementType()
                    ?? property.PropertyType.GetNullableElementTypeOrSelf();

            if (this.Configuration.EnumTypes.Contains(type))
            {
                return ClientFileType.Enum;
            }
            else if (this.Configuration.ClassTypes.Contains(type))
            {
                return ClientFileType.Class;
            }
            else if (this.Configuration.StructTypes.Contains(type))
            {
                return ClientFileType.Struct;
            }
            else
            {
                return base.GetFileType(property);
            }
        }
    }
}
