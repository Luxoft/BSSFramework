using Framework.DomainDriven.DAL.Revisions;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public class ReadOnlyNHibSession : NHibSessionBase
{
    private bool closed;
    private readonly ITransaction transaction;

    public ReadOnlyNHibSession(NHibSessionEnvironment environment)
            : base(environment, DBSessionMode.Read)
    {
        this.NativeSession = this.Environment.InternalSessionFactory.OpenSession();
        this.NativeSession.FlushMode = FlushMode.Manual;
        this.NativeSession.DefaultReadOnly = true;

        // need for support different isolation level (aka Snapshot)
        this.transaction = this.NativeSession.BeginTransaction();
    }

    public override bool Closed => this.closed;

    public sealed override ISession NativeSession { get; }


    public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic()
    {
        yield break;
    }

    public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>()
    {
        yield break;
    }

    public override void AsFault()
    {
    }

    public override void AsReadOnly()
    {
    }

    public override void AsWritable()
    {
        throw new InvalidOperationException("Readonly session already created");
    }

    public override async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (this.closed)
        {
            return;
        }

        this.closed = true;


        using (this.NativeSession)
        {
            using (this.transaction)
            {
            }
        }
    }

    public override async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException();
    }

    public override void RegisterModified<T>(T @object, ModificationType modificationType)
    {
        throw new ArgumentException($"Not supported: '{nameof(this.RegisterModified)}' for {this.GetType()}");
    }
}
