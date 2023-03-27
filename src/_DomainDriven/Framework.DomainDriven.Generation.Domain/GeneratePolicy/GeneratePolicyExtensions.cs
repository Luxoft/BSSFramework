using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation.Domain;

/// <summary>
/// Расширения для работы с политиками генерации
/// </summary>
public static class GeneratePolicyExtensions
{
    /// <summary>
    /// Исключение из генерации по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, TIdent, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new FuncGeneratePolicy<TIdent>((domainType, identity) => !filter(domainType, identity) && policy.Used(domainType, identity));
    }

    /// <summary>
    /// Исключение из генерации по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<TIdent, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return policy.Except((domainType, identity) => filter(identity));
    }

    /// <summary>
    /// Исключение из генерации по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return policy.Except((domainType, identity) => filter(domainType));
    }

    /// <summary>
    /// Исключение из генерации по доменным типам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="firstType">Первый доменный тип</param>
    /// <param name="otherTypes">Прочие доменные типы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, Type firstType, [NotNull] params Type[] otherTypes)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (firstType == null) throw new ArgumentNullException(nameof(firstType));
        if (otherTypes == null) { throw new ArgumentNullException(nameof(otherTypes)); }

        return policy.Except(new[] { firstType }.Concat(otherTypes));
    }

    /// <summary>
    /// Исключение из генерации по идентификаторам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="firstIdent">Первый идентификатор</param>
    /// <param name="otherIdents">Прочие идентификаторы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, TIdent firstIdent, params TIdent[] otherIdents)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (firstIdent == null) throw new ArgumentNullException(nameof(firstIdent));
        if (otherIdents == null) throw new ArgumentNullException(nameof(otherIdents));

        return policy.Except(new[] { firstIdent }.Concat(otherIdents));
    }

    /// <summary>
    /// Исключение из генерации по доменным типам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="types">Доменные типы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<Type> types)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (types == null) throw new ArgumentNullException(nameof(types));

        var hash = types.ToHashSet();

        return policy.Except(domainType => hash.Contains(domainType));
    }

    /// <summary>
    /// Исключение из генерации по идентификаторам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="idents">Идентификаторы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Except<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<TIdent> idents)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        var hash = idents.ToHashSet();

        return policy.Except(identity => hash.Contains(identity));
    }

    /// <summary>
    /// Добавление в генерацию по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, TIdent, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new FuncGeneratePolicy<TIdent>((domainType, identity) => filter(domainType, identity) || policy.Used(domainType, identity));
    }

    /// <summary>
    /// Добавление в генерацию по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<TIdent, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return policy.Add((domainType, identity) => filter(identity));
    }

    /// <summary>
    /// Добавление в генерацию по фильтру
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Func<Type, bool> filter)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return policy.Add((domainType, identity) => filter(domainType));
    }

    /// <summary>
    /// Добавление в генерацию по доменным типам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="firstType">Первый доменный тип</param>
    /// <param name="otherTypes">Прочие доменные типы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, Type firstType, [NotNull] params Type[] otherTypes)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (firstType == null) throw new ArgumentNullException(nameof(firstType));
        if (otherTypes == null) { throw new ArgumentNullException(nameof(otherTypes)); }

        return policy.Add(new[] { firstType }.Concat(otherTypes));
    }

    /// <summary>
    /// Добавление в генерацию по идентификаторам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="firstIdent">Первый идентификатор</param>
    /// <param name="otherIdents">Прочие идентификаторы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, TIdent firstIdent, params TIdent[] otherIdents)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (firstIdent == null) throw new ArgumentNullException(nameof(firstIdent));
        if (otherIdents == null) throw new ArgumentNullException(nameof(otherIdents));

        return policy.Add(new[] { firstIdent }.Concat(otherIdents));
    }

    /// <summary>
    /// Добавление в генерацию по доменным типам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="types">Доменные типы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<Type> types)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (types == null) throw new ArgumentNullException(nameof(types));

        var hash = types.ToHashSet();

        return policy.Add(domainType => hash.Contains(domainType));
    }

    /// <summary>
    /// Добавление в генерацию по идентификаторам
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy">Политика генерации</param>
    /// <param name="idents">Идентификаторы</param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Add<TIdent>(this IGeneratePolicy<TIdent> policy, IEnumerable<TIdent> idents)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        var hash = idents.ToHashSet();

        return policy.Add(identity => hash.Contains(identity));
    }

    /// <summary>
    /// Кеширование политики
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> WithCache<TIdent>(this IGeneratePolicy<TIdent> policy)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));

        return new CachedGeneratePolicy<TIdent>(policy);
    }

    /// <summary>
    /// Агрегация политик, для выполенения условий требуется выполнение любой политики
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Any<TIdent>(this IEnumerable<IGeneratePolicy<TIdent>> policies)
    {
        if (policies == null) throw new ArgumentNullException(nameof(policies));

        return policies.Aggregate (GeneratePolicy<TIdent>.DisableAll, (pol1, pol2) => pol1.Or(pol2));
    }

    /// <summary>
    /// Агрегация политик, для выполенения условий требуется выполнение любой из 2-х политик
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <param name="otherPolicy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> Or<TIdent>(this IGeneratePolicy<TIdent> policy, IGeneratePolicy<TIdent> otherPolicy)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (otherPolicy == null) throw new ArgumentNullException(nameof(otherPolicy));

        return new FuncGeneratePolicy<TIdent>((domainType, fileType) =>

                                                      policy.Used(domainType, fileType) || otherPolicy.Used(domainType, fileType));
    }

    /// <summary>
    /// Агрегация политик, для выполенения условий требуется выполнение всех политики
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policies"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> All<TIdent>(this IEnumerable<IGeneratePolicy<TIdent>> policies)
    {
        if (policies == null) throw new ArgumentNullException(nameof(policies));

        return policies.Aggregate(GeneratePolicy<TIdent>.AllowAll, (pol1, pol2) => pol1.And(pol2));
    }

    /// <summary>
    /// Агрегация политик, для выполенения условий требуется выполнение обеих политик
    /// </summary>
    /// <typeparam name="TIdent"></typeparam>
    /// <param name="policy"></param>
    /// <param name="otherPolicy"></param>
    /// <returns></returns>
    public static IGeneratePolicy<TIdent> And<TIdent>(this IGeneratePolicy<TIdent> policy, IGeneratePolicy<TIdent> otherPolicy)
    {
        if (policy == null) throw new ArgumentNullException(nameof(policy));
        if (otherPolicy == null) throw new ArgumentNullException(nameof(otherPolicy));

        return new FuncGeneratePolicy<TIdent>((domainType, fileType) =>

                                                      policy.Used(domainType, fileType) && otherPolicy.Used(domainType, fileType));
    }
}
