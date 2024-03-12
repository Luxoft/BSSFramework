using Framework.Core;

namespace Framework.DomainDriven.BLL;

public interface IBLLBaseContext : IBLLOperationEventContext,
                                   IODataBLLContext,
                                   IServiceProviderContainer
{
}
