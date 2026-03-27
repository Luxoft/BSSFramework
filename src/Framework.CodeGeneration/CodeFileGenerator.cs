using Framework.CodeDom;
using Framework.CodeDom.Rendering;
using Framework.CodeGeneration.Configuration._Container;
using Framework.FileGeneration;

namespace Framework.CodeGeneration;

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
