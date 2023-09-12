using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

/// <summary>
/// TODO: Remove after update to NET8.0 https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8#keyed-di-services
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TSecurityOperationCode"></typeparam>
public interface INotImplementedDomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>, INotImplementedDomainSecurityService<TDomainObject>
    where TSecurityOperationCode : struct, Enum
{

}

public interface INotImplementedDomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>
{
}
