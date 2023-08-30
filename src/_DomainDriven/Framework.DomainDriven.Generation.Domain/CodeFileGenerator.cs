using Framework.CodeDom;

namespace Framework.DomainDriven.Generation.Domain;

/// <summary>
/// Генератор cs-файлов с кодом
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
public abstract class CodeFileGenerator<TConfiguration> : GeneratorConfigurationContainer<TConfiguration>, IFileGenerator<ICodeFile, CodeDomRenderer>
        where TConfiguration : class
{
    protected CodeFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }

    public virtual CodeDomRenderer Renderer { get; } = CodeDomRenderer.CSharp;

    public IEnumerable<ICodeFile> GetFileGenerators()
    {
        return this.GetInternalFileGenerators();
    }

    protected abstract IEnumerable<ICodeFile> GetInternalFileGenerators();
}
