namespace Framework.Core.TypeResolving;

public interface ITypeResolverContainer<in T>
{
    ITypeResolver<T> TypeResolver { get; }
}
