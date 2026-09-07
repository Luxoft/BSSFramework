using System.Data;

using Anch.Core;

using Framework.Database.NHibernate.DAL.Revisions;
using Framework.Database.NHibernate.Envers;

using NHibernate;

namespace Framework.Database.NHibernate.Sessions;

public abstract class NHibSessionBase : INHibSession
{
    public abstract bool Closed { get; }

    public abstract DBSessionMode SessionMode { get; }

    public abstract IDbTransaction Transaction { get; }

    public IAuditReaderPatched AuditReader => field ??= new AuditReaderPatchedFactory(this.NativeSession).Create();

    public abstract ISession NativeSession { get; }

    public abstract Task FlushAsync(CancellationToken ct);

    /// <inheritdoc />
    public long GetCurrentRevision() => this.AuditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;

    /// <inheritdoc />
    public long GetMaxRevision() => this.AuditReader.GetMaxRevision();

    public abstract void AsFault();

    /// <inheritdoc />
    public abstract void AsReadOnly();

    /// <inheritdoc />
    public abstract void AsWritable();

    public abstract Task CloseAsync(CancellationToken ct);

    public async ValueTask DisposeAsync() => await this.CloseAsync(CancellationToken.None);
}
