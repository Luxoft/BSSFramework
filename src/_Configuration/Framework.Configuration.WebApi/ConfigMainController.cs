using Framework.Configuration.Generated.DTO;
using Framework.Infrastructure;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Framework.Configuration.WebApi;

public abstract partial class ConfigMainController : ApiControllerBase<IConfigurationBLLContext, IConfigurationDTOMappingService>;
