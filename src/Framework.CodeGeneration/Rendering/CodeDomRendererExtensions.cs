using CommonFramework;

using Framework.Core;
using Framework.Core.Rendering;
using Framework.FileGeneration;

namespace Framework.CodeGeneration.Rendering;

public static class CodeDomRendererExtensions
{
    public static GeneratedFileInfo RenderFile<TSource>(this IFileRenderer<TSource, string> renderer, string filename, TSource source)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (filename == null) throw new ArgumentNullException(nameof(filename));

        return new GeneratedFileInfo(filename + "." + renderer.FileExtension, renderer.Render(source));
    }

    public static GeneratedFileInfo RenderFile<TSource>(this IFileRenderer<TSource, string> renderer, string filename, IEnumerable<TSource> sources)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (filename == null) throw new ArgumentNullException(nameof(filename));
        if (sources == null) throw new ArgumentNullException(nameof(sources));

        return new GeneratedFileInfo(filename + "." + renderer.FileExtension, sources.Join(Environment.NewLine, renderer.Render));
    }
}
