using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationInfoSource(IEnumerable<FullSecurityOperation> securityOperations) : ISecurityOperationInfoSource
{
    private readonly IReadOnlyDictionary<SecurityOperation, SecurityOperationInfo> dict = securityOperations.ToDictionary(
        pair => pair.SecurityOperation,
        pair => pair.Info);

    public SecurityOperationInfo GetSecurityOperationInfo(SecurityOperation securityOperation) =>
        this.dict.GetValueOrDefault(securityOperation, () => new SecurityOperationInfo());
}
