namespace Framework.Database.EntityFramework.Sessions;

public record EfCurrentRevisionState
{
    public long CurrentRevision { get; set; }
}
