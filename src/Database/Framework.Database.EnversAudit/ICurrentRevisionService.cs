namespace Framework.Database.EnversAudit;

public interface IRevisionService
{
    /// <summary>
    /// Getting the current revision from the audit (returns 0 for now if called before the session flush)
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();

    /// <summary>
    /// Gets the maximum audit revision.
    /// </summary>
    /// <returns>System.Int64.</returns>
    long GetMaxRevision();
}
