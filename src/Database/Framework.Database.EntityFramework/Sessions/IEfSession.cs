using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework.Sessions;

public interface IEfSession : IDBSession
{
    DbContext NativeSession { get; }
}
