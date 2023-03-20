namespace Framework.Core;

public interface IRenderer<in TSource, out TResult>
{
    TResult Render(TSource source);
}
