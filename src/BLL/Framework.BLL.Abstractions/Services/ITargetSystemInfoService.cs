using Framework.BLL.Domain.TargetSystem;

namespace Framework.BLL.Services;

public interface ITargetSystemInfoService
{
    PersistentTargetSystemInfo GetPersistentTargetSystemInfo(Type domainType);
}
