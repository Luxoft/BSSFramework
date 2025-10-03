using System.Data;

using CommonFramework;

using Framework.DomainDriven.DAL.Revisions;

using NHibernate;
using NHibernate.Envers.Patch;

namespace Framework.DomainDriven.NHibernate;

public abstract class NHibSessionBase : INHibSession
{
    private Lazy<IAuditReaderPatched> lazyAuditReader { get; }

    internal NHibSessionBase(NHibSessionEnvironment environment, DBSessionMode sessionMode)
    {
        this.Environment = environment;
        this.SessionMode = sessionMode;

        this.lazyAuditReader = LazyHelper.Create(() => this.NativeSession.GetAuditReader());
    }

    public abstract bool Closed { get; }

    public DBSessionMode SessionMode { get; }

    public abstract IDbTransaction Transaction { get; }

    public IAuditReaderPatched AuditReader => this.lazyAuditReader.Value;

    public abstract ISession NativeSession { get; }

    protected internal NHibSessionEnvironment Environment { get; }

    public abstract Task FlushAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public long GetCurrentRevision()
    {
        return this.AuditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;
    }

    /// <inheritdoc />
    public long GetMaxRevision()
    {
        return this.AuditReader.GetMaxRevision();
    }

    public abstract void AsFault();

    /// <inheritdoc />
    public abstract void AsReadOnly();

    /// <inheritdoc />
    public abstract void AsWritable();

    public abstract Task CloseAsync(CancellationToken cancellationToken = default);

    public async ValueTask DisposeAsync()
    {
        await this.CloseAsync();
    }
}
