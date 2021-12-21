using System;

using JetBrains.Annotations;

namespace Framework.Graphviz
{
    public static class DotVisualizerExtensions
    {
        public static IDotVisualizer<TNewInput> OverrideInput<TOldInput, TNewInput>([NotNull] this IDotVisualizer<TOldInput> visualizer, [NotNull] Func<TNewInput, TOldInput> selector)
        {
            if (visualizer == null) throw new ArgumentNullException(nameof(visualizer));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new FuncDotVisualizer<TNewInput>((input, format) => visualizer.Render(selector(input), format));
        }

        private class FuncDotVisualizer<TNewInput> : IDotVisualizer<TNewInput>
        {
            private readonly Func<TNewInput, GraphvizFormat, byte[]> _func;


            public FuncDotVisualizer(Func<TNewInput, GraphvizFormat, byte[]> func)
            {
                if (func == null) throw new ArgumentNullException(nameof(func));

                this._func = func;
            }

            public byte[] Render(TNewInput dot, GraphvizFormat format)
            {
                return this._func(dot, format);
            }
        }
    }
}
