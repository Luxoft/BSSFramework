#nullable enable

namespace Framework.SecuritySystem;

public interface ISecurityOperationResolver
{
    SecurityOperation? TryGetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode);
}

public static class SecurityOperationResolverExtensions
{
    public static SecurityOperation GetSecurityOperation<TDomainObject>(
        this ISecurityOperationResolver resolver,
        BLLSecurityMode securityMode) =>
        resolver.TryGetSecurityOperation<TDomainObject>(securityMode)
        ?? throw new Exception($"SecurityOperation with mode {securityMode} not founded for type {typeof(TDomainObject).Name}");
}
