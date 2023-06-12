using System.Diagnostics.CodeAnalysis;

namespace Framework.DomainDriven.Repository;

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IRepositoryFactory<TDomainObject> : ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject, Guid>
{
}

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IRepositoryFactory<TDomainObject, TSecurityOperationCode> : IRepositoryFactory<TDomainObject>, ITemplateGenericRepositoryFactory<IRepository<TDomainObject>, TDomainObject, Guid, TSecurityOperationCode>
    where TSecurityOperationCode : struct, Enum
{
}
