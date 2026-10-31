namespace Framework.Database;

public interface IDBSessionManager
{
    /// <summary>
    /// Attempt to close the current db session (if it exists) and invoke the mechanism of flushing events to DALListeners
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task TryCloseDbSessionAsync(CancellationToken ct);

    /// <summary>
    ///  Attempt to mark the current db session (if it exists) that an error occurred and nothing should be written to the database.
    /// </summary>
    void TryFaultDbSession();
}