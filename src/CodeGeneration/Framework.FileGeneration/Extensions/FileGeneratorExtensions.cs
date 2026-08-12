using Framework.Core.Rendering;
using Framework.FileGeneration.Checkout;

namespace Framework.FileGeneration.Extensions;

public static class FileGeneratorExtensions
{
    public static IEnumerable<GeneratedFileInfo> Generate<TRenderData>(
        this IFileGenerator<IRenderingFile<TRenderData>, IFileRenderer<TRenderData, string>> generator,
        string path,
        ICheckOutService? checkOutService = null) =>
        generator.GetFileGenerators()
                 .Select(fileFactory => new GeneratedFileInfo(fileFactory.Filename + "." + generator.Renderer.FileExtension, generator.Renderer.Render(fileFactory)))
                 .Select(fileInfo => fileInfo.WithSave(path, checkOutService))
                 .ToList();
}
