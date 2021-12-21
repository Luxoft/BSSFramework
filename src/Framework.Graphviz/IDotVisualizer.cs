namespace Framework.Graphviz
{
    public interface IDotVisualizer<in TInput>
    {
        byte[] Render(TInput dot, GraphvizFormat format);
    }
}