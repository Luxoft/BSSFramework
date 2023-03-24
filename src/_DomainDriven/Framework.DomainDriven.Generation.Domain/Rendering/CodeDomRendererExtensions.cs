using System;
using System.Collections.Generic;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation.Domain;

public static class CodeDomRendererExtensions
{
    public static FileInfo RenderFile<TSource>([NotNull] this IFileRenderer<TSource, string> renderer, [NotNull] string filename, TSource source)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (filename == null) throw new ArgumentNullException(nameof(filename));

        return new FileInfo(filename + "." + renderer.FileExtension, renderer.Render(source));
    }

    public static FileInfo RenderFile<TSource>([NotNull] this IFileRenderer<TSource, string> renderer, [NotNull] string filename, IEnumerable<TSource> sources)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (filename == null) throw new ArgumentNullException(nameof(filename));
        if (sources == null) throw new ArgumentNullException(nameof(sources));

        return new FileInfo(filename + "." + renderer.FileExtension, sources.Join(Environment.NewLine, renderer.Render));
    }
}
