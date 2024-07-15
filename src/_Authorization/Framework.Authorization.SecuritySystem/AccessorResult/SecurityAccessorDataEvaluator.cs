using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityAccessorDataEvaluator(ISecurityAccessorInfinityStorage infinityStorage)
    : ISecurityAccessorDataEvaluator
{
    public virtual IEnumerable<string> Evaluate(SecurityAccessorData securityAccessorData)
    {
        switch (securityAccessorData)
        {
            case SecurityAccessorData.FixedSecurityAccessorData fixedResult:
                return fixedResult.Items;

            case SecurityAccessorData.AndSecurityAccessorData
            {
                Right: SecurityAccessorData.NegateSecurityAccessorData right
            } andNegateResult:
                return this.Evaluate(andNegateResult.Left).Except(
                    this.Evaluate(right.InnerData),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.AndSecurityAccessorData andResult:
                return this.Evaluate(andResult.Left).Intersect(
                    this.Evaluate(andResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.OrSecurityAccessorData orResult:
                return this.Evaluate(orResult.Left).Union(
                    this.Evaluate(orResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.InfinitySecurityAccessorData:
                return infinityStorage.GetInfinityData();

            case SecurityAccessorData.NegateSecurityAccessorData negateResult:
                return infinityStorage.GetInfinityData().Except(this.Evaluate(negateResult.InnerData));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityAccessorData));
        }
    }
}
