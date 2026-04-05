namespace Framework.Subscriptions.Metadata;

public interface IDomainObjectConverter<in TSource, out TTarget>
{
    TTarget Convert(TSource source);
}
