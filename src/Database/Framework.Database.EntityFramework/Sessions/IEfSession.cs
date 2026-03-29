using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public interface IEfSession : IDBSession
{
    DbContext NativeSession { get; }
}
