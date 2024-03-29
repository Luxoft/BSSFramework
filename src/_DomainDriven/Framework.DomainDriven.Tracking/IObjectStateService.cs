﻿namespace Framework.DomainDriven.Tracking;

public interface IObjectStateService
{
    IEnumerable<ObjectState> GetModifiedObjectStates(object value);

    bool IsNew(object entity);

    bool IsRemoving(object entity);
}
