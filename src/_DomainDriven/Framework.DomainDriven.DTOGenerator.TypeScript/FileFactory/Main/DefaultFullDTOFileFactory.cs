using System;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main
{
    /// <summary>
    /// Default fullDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultFullDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultFullDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override MainDTOFileType FileType => DTOGenerator.FileType.FullDTO;
    }
}
