using System.Collections.Generic;

namespace Framework.DomainDriven.BLL.Tracking;

public interface IObjectStateService
{
    IEnumerable<ObjectState> GetModifiedObjectStates(object value);

    bool IsNew(object entity);

    bool IsRemoving(object entity);
}
