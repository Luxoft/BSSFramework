using Framework.Core;

namespace Framework.SecuritySystem.Builders._Factory;

public abstract class FilterBuilderFactoryBase<TDomainObject, TBuilder>
{
    public virtual TBuilder CreateBuilder(
        SecurityPath<TDomainObject> baseSecurityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        var securityPathType = baseSecurityPath.GetType();

        if (baseSecurityPath is SecurityPath<TDomainObject>.ConditionPath conditionPath)
        {
            return this.CreateBuilder(conditionPath);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            return this.CreateBuilder(andSecurityPath, restrictionFilterInfoList);
        }
        else if (baseSecurityPath is SecurityPath<TDomainObject>.OrSecurityPath orSecurityPath)
        {
            return this.CreateBuilder(orSecurityPath, restrictionFilterInfoList);
        }
        else if (securityPathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            return new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, IReadOnlyList<SecurityContextRestrictionFilterInfo>, TBuilder>(this.CreateBuilder)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath, restrictionFilterInfoList);
        }
        else if (securityPathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<>))))
        {
            return new Func<SecurityPath<TDomainObject>, IReadOnlyList<SecurityContextRestrictionFilterInfo>, TBuilder>(
                       this.CreateSecurityContextBuilder<ISecurityContext>)
                   .CreateGenericMethod(securityPathType.GetGenericArguments().Skip(1).ToArray())
                   .Invoke<TBuilder>(this, baseSecurityPath, restrictionFilterInfoList);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(baseSecurityPath));
        }
    }

    protected abstract TBuilder CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath);

    protected abstract TBuilder CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext;

    protected abstract TBuilder CreateBuilder(
        SecurityPath<TDomainObject>.OrSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList);

    protected abstract TBuilder CreateBuilder(
        SecurityPath<TDomainObject>.AndSecurityPath securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList);

    protected abstract TBuilder CreateBuilder<TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList);

    private TBuilder CreateSecurityContextBuilder<TSecurityContext>(
        SecurityPath<TDomainObject> securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
        where TSecurityContext : class, ISecurityContext
    {
        var restrictionFilterInfo =
            (SecurityContextRestrictionFilterInfo<TSecurityContext>?)restrictionFilterInfoList.SingleOrDefault(
                v => v.SecurityContextType == typeof(TSecurityContext));

        return securityPath switch
        {
            SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> singleSecurityPath => this.CreateBuilder(
                singleSecurityPath,
                restrictionFilterInfo),
            SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> manySecurityPath => this.CreateBuilder(
                manySecurityPath,
                restrictionFilterInfo),
            _ => throw new ArgumentOutOfRangeException(nameof(securityPath))
        };
    }
}
