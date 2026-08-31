using System.Data;

using Framework.Core;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public class ReadOnlyNHibSession : IDBSession<ISession>
{
    private static readonly IDbTransaction DbTransaction = LazyInterfaceImplementHelper.CreateNotImplemented<IDbTransaction>("Readonly session");

    private readonly ITransaction transaction;

    public ReadOnlyNHibSession(NHibSessionEnvironment environment)
    {
        this.NativeSession = environment.InternalSessionFactory.OpenSession();
        this.NativeSession.FlushMode = FlushMode.Manual;
        this.NativeSession.DefaultReadOnly = true;

        // need for support different isolation level (aka Snapshot)
        this.transaction = this.NativeSession.BeginTransaction();
    }

    public DBSessionMode SessionMode { get; } = DBSessionMode.Read;

    public ISession NativeSession { get; }

    public IDbTransaction Transaction { get; } = DbTransaction;

    public bool Closed { get; private set; }

    public void AsFault()
    {
    }

    public void AsReadOnly()
    {
    }

    public void AsWritable() => throw new InvalidOperationException("Readonly session already created");

    public async Task CloseAsync(CancellationToken ct)
    {
        if (this.Closed)
        {
            return;
        }

        this.Closed = true;


        using (this.NativeSession)
        {
            using (this.transaction)
            {
            }
        }
    }

    public async Task FlushAsync(CancellationToken ct) => throw new InvalidOperationException("Readonly session cannot be flushed");

    public async ValueTask DisposeAsync() => await this.CloseAsync(CancellationToken.None);
}
