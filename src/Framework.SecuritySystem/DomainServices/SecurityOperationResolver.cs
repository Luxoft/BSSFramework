#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationResolver : ISecurityOperationResolver
{
    private readonly IReadOnlyDictionary<Type, SecurityOperation> viewDict;

    private readonly IReadOnlyDictionary<Type, SecurityOperation> editDict;

    public SecurityOperationResolver(IEnumerable<DomainObjectSecurityOperationInfo> infos)
    {
        var cached = infos.ToList();

        this.viewDict = GetDict(cached, info => info.ViewOperation);
        this.editDict = GetDict(cached, info => info.ViewOperation);
    }

    public SecurityOperation? TryGetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
    {
        switch (securityMode)
        {
            case BLLSecurityMode.Disabled:
                return new DisabledSecurityOperation();

            case BLLSecurityMode.View:
                return this.viewDict.GetValueOrDefault(typeof(TDomainObject));

            case BLLSecurityMode.Edit:
                return this.editDict.GetValueOrDefault(typeof(TDomainObject));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityMode));
        }
    }

    private static Dictionary<Type, SecurityOperation> GetDict(IEnumerable<DomainObjectSecurityOperationInfo> infos, Func<DomainObjectSecurityOperationInfo, SecurityOperation?> selector)
    {
        var request = from info in infos

                      let securityOperation = selector(info)

                      where securityOperation != null

                      select info.DomainType.ToKeyValuePair(securityOperation);


        return request.ToDictionary();
    }
}
