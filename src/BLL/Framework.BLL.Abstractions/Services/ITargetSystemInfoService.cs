using Framework.BLL.Domain.TargetSystem;

namespace Framework.BLL.Services;

public interface ITargetSystemInfoService
{
    TargetSystemInfo GetTargetSystemInfo(Type domainType);

    PersistentTargetSystemInfo GetPersistentTargetSystemInfo(Type domainType);
}
