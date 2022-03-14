using System.Collections.Generic;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IServiceEnvironmentModule<in TBLLContextContainer>
{
    IEnumerable<IDALListener> GetDALFlushedListeners(TBLLContextContainer container);

    IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners(TBLLContextContainer container)
    {
        yield break;
    }
}
