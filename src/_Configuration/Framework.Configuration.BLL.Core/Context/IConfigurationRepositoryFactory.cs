using System.Diagnostics.CodeAnalysis;

using Framework.Configuration.Domain;
using Framework.DomainDriven.Repository;

namespace Framework.Configuration.BLL.Core.Context;

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IConfigurationRepositoryFactory<TDomainObject>
    : IRepositoryFactory<TDomainObject, Guid, ConfigurationSecurityOperationCode>
    where TDomainObject : PersistentDomainObjectBase
{
}
