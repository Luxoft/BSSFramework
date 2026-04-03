using System.Data;

using CommonFramework;

using Framework.Database.NHibernate.DAL.Revisions;
using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public abstract class NHibSessionBase : INHibSession
{
    private Lazy<IAuditReaderPatched> LazyAuditReader { get; }

    internal NHibSessionBase(NHibSessionEnvironment environment, DBSessionMode sessionMode)
    {
        this.Environment = environment;
        this.SessionMode = sessionMode;

        this.LazyAuditReader = LazyHelper.Create(() => this.NativeSession.GetAuditReader());
    }

    public abstract bool Closed { get; }

    public DBSessionMode SessionMode { get; }

    public abstract IDbTransaction Transaction { get; }

    public IAuditReaderPatched AuditReader => this.LazyAuditReader.Value;

    public abstract ISession NativeSession { get; }

    protected internal NHibSessionEnvironment Environment { get; }

    public abstract Task FlushAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public long GetCurrentRevision() => this.AuditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;

    /// <inheritdoc />
    public long GetMaxRevision() => this.AuditReader.GetMaxRevision();

    public abstract void AsFault();

    /// <inheritdoc />
    public abstract void AsReadOnly();

    /// <inheritdoc />
    public abstract void AsWritable();

    public abstract Task CloseAsync(CancellationToken cancellationToken = default);

    public async ValueTask DisposeAsync() => await this.CloseAsync();
}
