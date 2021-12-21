using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService
{
    /// <summary>
    /// Client code type reference service
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class ClientCodeTypeReferenceService<TConfiguration> : ConfigurationCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public ClientCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
        {
        }

        public override CodeTypeReference GetCodeTypeReferenceByType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var genericNullableType = type.GetNullableElementType();

            if (genericNullableType != null)
            {
                return this.GetCodeTypeReferenceByType(genericNullableType).ToNullableReference();
            }

            if (type == typeof(Guid))
            {
                return CodeExpressionHelper.GetGuidCodeTypeReference();
            }

            if (type.IsEnum)
            {
                return this.Configuration.GetCodeTypeReference(type, ClientFileType.Enum);
            }

            if (!type.IsPrimitiveType())
            {
                if (type.IsClass)
                {
                    return this.Configuration.GetCodeTypeReference(type, ClientFileType.Class);
                }

                if (type.IsValueType)
                {
                    return this.Configuration.GetCodeTypeReference(type, ClientFileType.Struct);
                }
            }

            return base.GetCodeTypeReferenceByType(type);
        }

        public override RoleFileType GetFileType(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var type = property.PropertyType.GetNullableElementTypeOrSelf();

            if (this.Configuration.EnumTypes.Contains(type))
            {
                return ClientFileType.Enum;
            }

            if (this.Configuration.ClassTypes.Contains(type))
            {
                return ClientFileType.Class;
            }

            if (this.Configuration.StructTypes.Contains(type))
            {
                return ClientFileType.Struct;
            }

            return base.GetFileType(property);
        }
    }
}
