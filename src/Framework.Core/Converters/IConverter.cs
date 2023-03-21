namespace Framework.Core;

public interface IConverter<in TSource, out TTarget>
{
    TTarget Convert(TSource source);
}
