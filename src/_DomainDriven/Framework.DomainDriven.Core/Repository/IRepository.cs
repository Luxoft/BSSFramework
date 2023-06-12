#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace Framework.DomainDriven.Repository;

[SuppressMessage("SonarQube", "S4023", Justification = "Interface to simplify working with Repository")]
public interface IRepository<TDomainObject> : IGenericRepository<TDomainObject, Guid>
{
}
