namespace Framework.BLL.Domain.DAL.Revisions;

public abstract class RevisionInfoBase(AuditRevisionType revisionType, string author, DateTime date, long revisionNumber)
{
    public AuditRevisionType RevisionType => revisionType;

    public string Author => author;

    public DateTime Date => date;

    public long RevisionNumber => revisionNumber;
}
