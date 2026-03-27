namespace Framework.Core.AnonymousTypeBuilder;

public interface IAnonymousObjectBuilder<in TSource>
{
    object GetAnonymousObject(TSource source);
}
