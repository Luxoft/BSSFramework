using System.Collections.Frozen;

using Framework.BLL.Domain.TargetSystem;

namespace Framework.BLL.Services;

public class TargetSystemInfoService(IEnumerable<PersistentTargetSystemInfo> targetSystemInfoList) : ITargetSystemInfoService
{
    private readonly FrozenDictionary<Type, PersistentTargetSystemInfo> cache =
        targetSystemInfoList.SelectMany(tsi => tsi.Domain.Types.Select(dt => (dt, tsi)))
                            .ToFrozenDictionary(pair => pair.dt.Type, pair => pair.tsi);

    public PersistentTargetSystemInfo GetPersistentTargetSystemInfo(Type domainType) =>
        this.cache.GetValueOrDefault(domainType)
        ?? throw new ArgumentOutOfRangeException(nameof(domainType), $"{nameof(PersistentTargetSystemInfo)} for domainType {domainType.Name} not found");
}
