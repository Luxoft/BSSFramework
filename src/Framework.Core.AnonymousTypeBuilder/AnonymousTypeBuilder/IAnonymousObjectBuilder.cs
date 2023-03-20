namespace Framework.Core;

public interface IAnonymousObjectBuilder<in TSource>
{
    object GetAnonymousObject(TSource source);
}
