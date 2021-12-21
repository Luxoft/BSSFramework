using System.Collections.Generic;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;

using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Facade;
using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript
{
    public class TypeScriptFacadeFileGenerator : TypeScriptFacadeFileGenerator<ITypeScriptFacadeGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>>
    {
        public TypeScriptFacadeFileGenerator(ITypeScriptFacadeGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class TypeScriptFacadeFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, ITypeScriptFacadeGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public TypeScriptFacadeFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }

        public override CodeDomRenderer Renderer { get; } = TypeScriptCodeDomRenderer.Default;


        protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
        {
            yield return new DefaultFacadeFileFactory<TConfiguration>(this.Configuration);
        }
    }
}
