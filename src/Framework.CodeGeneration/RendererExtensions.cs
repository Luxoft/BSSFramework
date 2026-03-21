using Framework.Core;

namespace Framework.DomainDriven.Generation;

public static class RendererExtensions
{
    public static TResult Render<TSource, TResult>(this IRenderer<TSource, TResult> renderer, IRenderingFile<TSource> renderingFile)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (renderingFile == null) throw new ArgumentNullException(nameof(renderingFile));

        return renderer.Render(renderingFile.GetRenderData());
    }
}
