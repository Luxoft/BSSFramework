using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation
{
    public static class RendererExtensions
    {
        public static TResult Render<TSource, TResult>([NotNull] this IRenderer<TSource, TResult> renderer, [NotNull] IRenderingFile<TSource> renderingFile)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            if (renderingFile == null) throw new ArgumentNullException(nameof(renderingFile));

            return renderer.Render(renderingFile.GetRenderData());
        }
    }
}
