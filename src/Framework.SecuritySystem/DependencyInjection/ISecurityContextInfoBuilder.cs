namespace Framework.SecuritySystem.DependencyInjection;

public interface ISecurityContextInfoBuilder
{
    ISecurityContextInfoBuilder Add<TSecurityContext>(Guid id, string? name = null, Func<TSecurityContext, string>? displayFunc = null)
        where TSecurityContext : ISecurityContext;
}
