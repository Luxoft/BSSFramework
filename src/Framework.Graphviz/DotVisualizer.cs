namespace Framework.Graphviz
{
    public abstract class DotVisualizer<TInput> : IDotVisualizer<TInput>
    {
        public abstract byte[] Render(TInput dot, GraphvizFormat format);


        private class EmptyDotVisualizer : DotVisualizer<TInput>
        {
            public override byte[] Render(TInput dot, GraphvizFormat format)
            {
                return new byte[0];
            }
        }


        public static readonly DotVisualizer<TInput> Empty = new EmptyDotVisualizer();
    }
}