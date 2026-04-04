namespace Framework.Subscriptions.Metadata;

public interface IRenderingObjectConverter<in TSource, out TTarget>
{
    TTarget Convert(TSource source);
}
