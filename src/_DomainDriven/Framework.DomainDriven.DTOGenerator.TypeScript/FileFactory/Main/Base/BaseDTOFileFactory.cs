using System;
using System.CodeDom;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base.ByProperty;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base
{
    /// <summary>
    /// IBaseDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public interface IBaseDTOFileFactory<out TConfiguration> : IPropertyFileFactory<TConfiguration, MainDTOFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
    }

    /// <summary>
    /// BaseDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class BaseDTOFileFactory<TConfiguration> : PropertyFileFactory<TConfiguration, MainDTOFileType>, IBaseDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        protected BaseDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public DTOFileType BaseType => this.FileType.GetBaseType(false);

        public override CodeTypeReference BaseReference
        {
            get { return this.BaseType.Maybe(baseType => this.Configuration.GetCodeTypeReference(this.DomainType, baseType)); }
        }

        protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(System.Reflection.PropertyInfo sourceProperty)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            foreach (var customAttribute in base.GetPropertyCustomAttributes(sourceProperty))
            {
                yield return customAttribute;
            }
        }
    }
}
