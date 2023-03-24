using System.Collections.Generic;

namespace Framework.DomainDriven.BLL.Tracking;

public class EmptyObjectStateService : IObjectStateService
{
    public static readonly EmptyObjectStateService Instance = new EmptyObjectStateService();


    private EmptyObjectStateService()
    {

    }


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
