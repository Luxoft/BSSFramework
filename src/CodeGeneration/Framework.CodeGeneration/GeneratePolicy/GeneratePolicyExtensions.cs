namespace Framework.CodeGeneration.GeneratePolicy;

/// <summary>
/// Extensions for working with generation policies
/// </summary>
public static class GeneratePolicyExtensions
{
    /// <summary>
    /// Exclude from generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, TIdent, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return new FuncGeneratePolicy<TIdent>((domainType, identity) => !filter(domainType, identity) && policy.Used(domainType, identity));
    }

    /// <summary>
    /// Exclude from generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<TIdent, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return policy.Except((domainType, identity) => filter(identity));
    }

    /// <summary>
    /// Exclude from generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return policy.Except((domainType, identity) => filter(domainType));
    }

    /// <summary>
    /// Exclude from generation by domain types
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="firstType">First domain type</param>
    /// <param name="otherTypes">Other domain types</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Type firstType, params Type[] otherTypes)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (firstType is null) throw new ArgumentNullException(nameof(firstType));
        if (otherTypes is null) { throw new ArgumentNullException(nameof(otherTypes)); }

        return policy.Except(new[] { firstType }.Concat(otherTypes));
    }

    /// <summary>
    /// Exclude from generation by identifiers
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="firstIdent">First identifier</param>
    /// <param name="otherIdents">Other identifiers</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, TIdent firstIdent, params TIdent[] otherIdents)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (firstIdent is null) throw new ArgumentNullException(nameof(firstIdent));
        if (otherIdents is null) throw new ArgumentNullException(nameof(otherIdents));

        return policy.Except(new[] { firstIdent }.Concat(otherIdents));
    }

    /// <summary>
    /// Exclude from generation by domain types
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="types">Domain types</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<Type> types)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (types is null) throw new ArgumentNullException(nameof(types));

        var hash = types.ToHashSet();

        return policy.Except(domainType => hash.Contains(domainType));
    }

    /// <summary>
    /// Exclude from generation by identifiers
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="idents">Identifiers</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<TIdent> idents)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (idents is null) throw new ArgumentNullException(nameof(idents));

        var hash = idents.ToHashSet();

        return policy.Except(identity => hash.Contains(identity));
    }

    /// <summary>
    /// Add to generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, TIdent, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return new FuncGeneratePolicy<TIdent>((domainType, identity) => filter(domainType, identity) || policy.Used(domainType, identity));
    }

    /// <summary>
    /// Add to generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<TIdent, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return policy.Add((domainType, identity) => filter(identity));
    }

    /// <summary>
    /// Add to generation by filter
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="filter">Filter</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, bool> filter)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        return policy.Add((domainType, identity) => filter(domainType));
    }

    /// <summary>
    /// Add to generation by domain types
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="firstType">First domain type</param>
    /// <param name="otherTypes">Other domain types</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Type firstType, params Type[] otherTypes)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (firstType is null) throw new ArgumentNullException(nameof(firstType));
        if (otherTypes is null) { throw new ArgumentNullException(nameof(otherTypes)); }

        return policy.Add(new[] { firstType }.Concat(otherTypes));
    }

    /// <summary>
    /// Add to generation by identifiers
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="firstIdent">First identifier</param>
    /// <param name="otherIdents">Other identifiers</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, TIdent firstIdent, params TIdent[] otherIdents)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (firstIdent is null) throw new ArgumentNullException(nameof(firstIdent));
        if (otherIdents is null) throw new ArgumentNullException(nameof(otherIdents));

        return policy.Add(new[] { firstIdent }.Concat(otherIdents));
    }

    /// <summary>
    /// Add to generation by domain types
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="types">Domain types</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<Type> types)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (types is null) throw new ArgumentNullException(nameof(types));

        var hash = types.ToHashSet();

        return policy.Add(domainType => hash.Contains(domainType));
    }

    /// <summary>
    /// Add to generation by identifiers
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Generation policy</param>
    /// <param name="idents">Identifiers</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<TIdent> idents)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (idents is null) throw new ArgumentNullException(nameof(idents));

        var hash = idents.ToHashSet();

        return policy.Add(identity => hash.Contains(identity));
    }

    /// <summary>
    /// Policy caching
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> WithCache<TIdent>(this IGeneratePolicy<TIdent> policy)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));

        return new CachedGeneratePolicy<TIdent>(policy);
    }

    /// <summary>
    /// Policy aggregation; any policy must be satisfied to meet the conditions
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Any<TIdent>(this IEnumerable<IGeneratePolicy<TIdent>> policies)
    {
        if (policies is null) throw new ArgumentNullException(nameof(policies));

        return policies.Aggregate(GeneratePolicy<TIdent>.DisableAll, (pol1, pol2) => pol1.Or(pol2));
    }

    /// <summary>
    /// Policy aggregation; either of the 2 policies must be satisfied to meet the conditions
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <param name="otherPolicy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Or<TIdent>(this IGeneratePolicy<TIdent> policy, IGeneratePolicy<TIdent> otherPolicy)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (otherPolicy is null) throw new ArgumentNullException(nameof(otherPolicy));

        return new FuncGeneratePolicy<TIdent>((domainType, fileType) =>

                                                      policy.Used(domainType, fileType) || otherPolicy.Used(domainType, fileType));
    }

    /// <summary>
    /// Policy aggregation; all policies must be satisfied to meet the conditions
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> All<TIdent>(this IEnumerable<IGeneratePolicy<TIdent>> policies)
    {
        if (policies is null) throw new ArgumentNullException(nameof(policies));

        return policies.Aggregate(GeneratePolicy<TIdent>.AllowAll, (pol1, pol2) => pol1.And(pol2));
    }

    /// <summary>
    /// Policy aggregation; both policies must be satisfied to meet the conditions
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <param name="otherPolicy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> And<TIdent>(this IGeneratePolicy<TIdent> policy, IGeneratePolicy<TIdent> otherPolicy)
    {
        if (policy is null) throw new ArgumentNullException(nameof(policy));
        if (otherPolicy is null) throw new ArgumentNullException(nameof(otherPolicy));

        return new FuncGeneratePolicy<TIdent>((domainType, fileType) =>

                                                      policy.Used(domainType, fileType) && otherPolicy.Used(domainType, fileType));
    }
}
