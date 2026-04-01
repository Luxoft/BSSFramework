namespace Framework.Tracking;

public class EmptyObjectStateService : IObjectStateService
{
    public IEnumerable<ObjectState> GetModifiedObjectStates(object value)
    {
        yield break;
    }

    public bool IsNew(object entity) => false;

    public bool IsRemoving(object entity) => false;
}
