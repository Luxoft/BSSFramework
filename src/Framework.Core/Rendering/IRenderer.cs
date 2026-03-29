namespace Framework.Core.Rendering;

public interface IRenderer<in TSource, out TResult>
{
    TResult Render(TSource source);
}
