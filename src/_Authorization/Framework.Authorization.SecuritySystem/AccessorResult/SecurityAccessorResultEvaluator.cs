using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityAccessorResultEvaluator(ISecurityAccessorInfinityStorage infinityStorage)
    : ISecurityAccessorResultEvaluator
{
    public virtual IEnumerable<string> GetAccessors(SecurityAccessorResult securityAccessorResult)
    {
        switch (securityAccessorResult)
        {
            case SecurityAccessorResult.FixedSecurityAccessorResult fixedResult:
                return fixedResult.Items;

            case SecurityAccessorResult.AndSecurityAccessorResult
            {
                Right: SecurityAccessorResult.NegateSecurityAccessorResult right
            } andNegateResult:
                return this.GetAccessors(andNegateResult.Left).Except(
                    this.GetAccessors(right.BaseResult),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorResult.AndSecurityAccessorResult andResult:
                return this.GetAccessors(andResult.Left).Intersect(
                    this.GetAccessors(andResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorResult.OrSecurityAccessorResult orResult:
                return this.GetAccessors(orResult.Left).Union(
                    this.GetAccessors(orResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorResult.InfinitySecurityAccessorResult:
                return infinityStorage.GetInfinityData();

            case SecurityAccessorResult.NegateSecurityAccessorResult negateResult:
                return infinityStorage.GetInfinityData().Except(this.GetAccessors(negateResult.BaseResult));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityAccessorResult));
        }
    }
}
