using System;
using System.CodeDom;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator
{
    public interface ICodeTypeReferenceService
    {
        CodeTypeReference GetCodeTypeReferenceByType(Type type);

        RoleFileType GetFileType(PropertyInfo property);
    }

    public abstract class CodeTypeReferenceService<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, ICodeTypeReferenceService
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        protected CodeTypeReferenceService(TConfiguration configuration)
            : base(configuration)
        {
        }


        public virtual CodeTypeReference GetCodeTypeReferenceByType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
            {
                return new CodeTypeReference(this.GetCodeTypeReferenceByType(type.GetElementType()), type.GetArrayRank());
            }
            else
            {
                return this.Configuration.DefaultCodeTypeReferenceService.GetCodeTypeReferenceByType(type);
            }
        }

        public virtual RoleFileType GetFileType(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return null;
        }
    }
}