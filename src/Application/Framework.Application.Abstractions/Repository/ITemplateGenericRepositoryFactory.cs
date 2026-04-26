using Anch.SecuritySystem;
using Anch.SecuritySystem.Providers;

namespace Framework.Application.Repository;

public interface ITemplateGenericRepositoryFactory<out TRepository, TDomainObject>
{
    /// <summary>
    /// Create new Repository without security filters
    /// </summary>
    TRepository Create();

    TRepository Create(SecurityRule securityRule);

    TRepository Create(ISecurityProvider<TDomainObject> securityProvider);
}
