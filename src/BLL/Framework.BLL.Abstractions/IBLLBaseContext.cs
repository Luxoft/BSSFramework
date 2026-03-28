using Framework.Core;

namespace Framework.BLL;

public interface IBLLBaseContext : IBLLOperationEventContext,
                                   IODataBLLContext,
                                   IServiceProviderContainer;
