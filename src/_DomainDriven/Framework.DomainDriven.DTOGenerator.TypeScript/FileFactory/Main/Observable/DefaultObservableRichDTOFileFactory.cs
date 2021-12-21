using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator.TypeScript.Extensions;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default observable richDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultObservableRichDTOFileFactory<TConfiguration> : ObservableDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultObservableRichDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override MainDTOFileType FileType => ObservableFileType.ObservableRichDTO;

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            foreach (var attributeDeclaration in this.DomainType.GetRestrictionCodeAttributeDeclarations())
            {
                yield return attributeDeclaration;
            }
        }
    }
}