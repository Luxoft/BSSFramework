using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Observable
{
    /// <summary>
    /// Default observable fullDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultObservableFullDTOFileFactory<TConfiguration> : ObservableDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultObservableFullDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(this.DomainType, ObservableFileType.ObservableSimpleDTO);

        public override MainDTOFileType FileType => ObservableFileType.ObservableFullDTO;
    }
}
