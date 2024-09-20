namespace Framework.DomainDriven.DAL.Revisions;

public abstract class RevisionInfoBase(AuditRevisionType revisionType, string author, DateTime date, long revisionNumber)
{
    public AuditRevisionType RevisionType
    {
        get { return revisionType; }
    }

    public string Author
    {
        get { return author; }
    }

    public DateTime Date
    {
        get { return date; }
    }

    public long RevisionNumber
    {
        get { return revisionNumber; }
    }
}
