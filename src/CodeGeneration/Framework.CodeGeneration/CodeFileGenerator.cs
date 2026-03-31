using Framework.CodeDom.Rendering;
using Framework.CodeGeneration.Configuration;
using Framework.FileGeneration;

namespace Framework.CodeGeneration;

/// <summary>
/// Генератор cs-файлов с кодом
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
public abstract class CodeFileGenerator<TConfiguration>(TConfiguration configuration)
    : FileGenerator<TConfiguration, ICodeFile, CodeDomRenderer>(configuration), IFileGenerator<ICodeFile, CodeDomRenderer>
    where TConfiguration : class, ICodeGeneratorConfiguration<ICodeGenerationEnvironment>
{
    public override CodeDomRenderer Renderer { get; } = CodeDomRenderer.CSharp;

    public sealed override IEnumerable<ICodeFile> GetFileGenerators() => this.GetInternalFileGenerators();

    protected abstract IEnumerable<ICodeFile> GetInternalFileGenerators();
}
