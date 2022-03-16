using System.Collections.Generic;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IBLLContextContainerModule
{
    IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
    {
        yield break;
    }

    void SubscribeEvents();
}
