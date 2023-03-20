namespace Framework.Core;

public interface ITypeResolverContainer<in T>
{
    ITypeResolver<T> TypeResolver { get; }
}
