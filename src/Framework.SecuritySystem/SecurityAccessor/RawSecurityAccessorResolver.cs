namespace Framework.SecuritySystem.SecurityAccessor;

public class RawSecurityAccessorResolver(ISecurityAccessorInfinityStorage infinityStorage) : ISecurityAccessorResolver
{
    public const string Key = "Raw";

    public virtual IEnumerable<string> Resolve(SecurityAccessorData securityAccessorData)
    {
        switch (securityAccessorData)
        {
            case SecurityAccessorData.FixedSecurityAccessorData fixedResult:
                return fixedResult.Items;

            case SecurityAccessorData.AndSecurityAccessorData
            {
                Right: SecurityAccessorData.NegateSecurityAccessorData right
            } andNegateResult:
                return this.Resolve(andNegateResult.Left).Except(
                    this.Resolve(right.InnerData),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.AndSecurityAccessorData andResult:
                return this.Resolve(andResult.Left).Intersect(
                    this.Resolve(andResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.OrSecurityAccessorData orResult:
                return this.Resolve(orResult.Left).Union(
                    this.Resolve(orResult.Right),
                    StringComparer.CurrentCultureIgnoreCase);

            case SecurityAccessorData.InfinitySecurityAccessorData:
                return infinityStorage.GetInfinityData();

            case SecurityAccessorData.NegateSecurityAccessorData negateResult:
                return infinityStorage.GetInfinityData().Except(this.Resolve(negateResult.InnerData));

            default:
                throw new ArgumentOutOfRangeException(nameof(securityAccessorData));
        }
    }
}
