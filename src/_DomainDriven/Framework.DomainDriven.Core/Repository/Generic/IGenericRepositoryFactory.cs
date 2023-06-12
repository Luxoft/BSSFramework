using System.Diagnostics.CodeAnalysis;

namespace Framework.DomainDriven.Repository;

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface
    IGenericRepositoryFactory<TDomainObject, TIdent> : ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>,
        TDomainObject>
{
}

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IGenericRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode> : IGenericRepositoryFactory<TDomainObject, TIdent>, ITemplateGenericRepositoryFactory<IGenericRepository<TDomainObject, TIdent>, TDomainObject, TSecurityOperationCode>
    where TSecurityOperationCode : struct, Enum
{
}
