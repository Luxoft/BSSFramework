namespace Framework.DomainDriven.Tracking;

public class EmptyObjectStateService : IObjectStateService
{
    public IEnumerable<ObjectState> GetModifiedObjectStates(object value)
    {
        yield break;
    }

    public bool IsNew(object entity)
    {
        return false;
    }

    public bool IsRemoving(object entity)
    {
        return false;
    }
}
