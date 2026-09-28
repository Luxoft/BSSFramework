namespace Framework.Database.NHibernate.EnversAudit.LinqVisitors;

internal class Immutable<T>
        where T : class
{
    private T source = null!;

    public T Value
    {
        get => this.source;

        set
        {
            if (this.source is not null && value != this.source)
            {
                throw new ArgumentException("Value also initialized");
            }

            this.source = value;
        }
    }

    public bool IsInit => this.source is not null;
}
