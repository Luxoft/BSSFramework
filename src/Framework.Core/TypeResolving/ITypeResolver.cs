namespace Framework.Core.TypeResolving;

public interface ITypeResolver<in T> : ITypeSource
{
    Type? TryResolve(T identity);

    Type Resolve(T identity) => this.TryResolve(identity) ?? throw new Exception($"Type \"{identity}\" can't be resolved.");
}
