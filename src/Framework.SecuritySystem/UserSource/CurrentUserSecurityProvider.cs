using System.Linq.Expressions;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.UserSource;

public class CurrentUserSecurityProvider<TDomainObject>(
    IServiceProvider serviceProvider,
    IUserPathInfo userPathInfo,
    SecurityRuleCredential securityRuleCredential,
    CurrentUserSecurityProviderRelativeKey? key = null) : ISecurityProvider<TDomainObject>
{
    private readonly Lazy<ISecurityProvider<TDomainObject>> lazyInnerProvider = LazyHelper.Create(
        () =>
        {
            var generics = new[] { typeof(TDomainObject), userPathInfo.UserDomainObjectType };

            var relativeDomainPathInfoType = typeof(IRelativeDomainPathInfo<,>).MakeGenericType(generics);

            var relativePathKey = key?.Name;

            var relativeDomainPathInfo = relativePathKey == null
                                             ? serviceProvider.GetRequiredService(relativeDomainPathInfoType)
                                             : serviceProvider.GetRequiredKeyedService(relativeDomainPathInfoType, relativePathKey);

            return (ISecurityProvider<TDomainObject>)
                ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    typeof(CurrentUserSecurityProvider<,>).MakeGenericType(generics),
                    relativeDomainPathInfo);
        });

    private ISecurityProvider<TDomainObject> InnerProvider => this.lazyInnerProvider.Value;

    public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) => this.InnerProvider.InjectFilter(queryable);

    public AccessResult GetAccessResult(TDomainObject domainObject) => this.InnerProvider.GetAccessResult(domainObject);

    public bool HasAccess(TDomainObject domainObject) => this.InnerProvider.HasAccess(domainObject);

    public SecurityAccessorData GetAccessorData(TDomainObject domainObject) => this.InnerProvider.GetAccessorData(domainObject);
}

public class CurrentUserSecurityProvider<TDomainObject, TUserDomainObject>(
    IRelativeDomainPathInfo<TDomainObject, TUserDomainObject> relativeDomainPathInfo,
    UserPathInfo<TUserDomainObject> userPathInfo,
    ICurrentUserSource<TUserDomainObject> currentUserSource) : SecurityProvider<TDomainObject>
    where TUserDomainObject : class
{
    public override Expression<Func<TDomainObject, bool>> SecurityFilter { get; } =
        relativeDomainPathInfo.Path.Select(user => currentUserSource.CurrentUser == user);

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
    {
        var user = relativeDomainPathInfo.Path.Eval(domainObject);

        return SecurityAccessorData.TryReturn(user == null ? null : userPathInfo.NamePath.Eval(user));
    }
}
