using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public static class GeneratePolicyExtensions
{
    public static IGeneratePolicy<MethodIdentity> Except(this IGeneratePolicy<MethodIdentity> policy, Func<MethodIdentityType, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return policy.Except(identity => filter(identity.Type));
    }

    public static IGeneratePolicy<MethodIdentity> Except(this IGeneratePolicy<MethodIdentity> policy, params MethodIdentityType[] idents)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        var hash = idents.ToHashSet();

        return policy.Except(identity => hash.Contains(identity));
    }
}
