namespace Framework.Database.EntityFramework.EnversAudit;

public record EfCurrentRevisionState
{
    public long CurrentRevision { get; set; }
}
