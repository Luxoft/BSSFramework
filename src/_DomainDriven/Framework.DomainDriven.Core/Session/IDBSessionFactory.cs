using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IDBSessionFactory : IFactory<DBSessionMode, IDBSession>, IDisposable
    {
        AvailableValues AvailableValues { get; }

        event EventHandler<SessionFlushedEventArgs> SessionFlushed;
    }
}
