using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;
using CommonFramework.GenericRepository;
using CommonFramework.IdentitySource;
using CommonFramework.VisualIdentitySource;

using HierarchicalExpand;

using SecuritySystem;
using SecuritySystem.ExternalSystem;
using SecuritySystem.GeneralPermission;
using SecuritySystem.Services;

namespace SampleSystem.IntegrationTests;

//[TestClass]
//public class InitTests : TestBase
//{
//    [TestMethod]
//    public async Task TestInit()
//    {
//        await this.RootServiceProvider.GetRequiredService<TestDataInitializer>().InitializeAsync(CancellationToken.None);
//    }
//}



//public class MyRawPermissionConverter<TPermissionRestriction, TSecurityContextObjectIdent>(
//    ISecurityContextSource securityContextSource,
//    IIdentityInfoSource identityInfoSource,
//    IRealTypeResolver realTypeResolver,
//    IPermissionRestrictionRawConverter<TPermissionRestriction> permissionRestrictionRawConverter) : IRawPermissionConverter<TPermissionRestriction>

//    where TPermissionRestriction : class
//    where TSecurityContextObjectIdent : notnull
//{
//    public Dictionary<Type, Array> ConvertPermission(DomainSecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<TPermissionRestriction> restrictions,
//        IEnumerable<Type> securityContextTypes)
//    {
//        var rawRestrictions = permissionRestrictionRawConverter.Convert(restrictions);

//        var filterInfoDict = securityRule.GetSafeSecurityContextRestrictionFilters().ToDictionary(filterInfo => filterInfo.SecurityContextType);

//        return securityContextTypes.ToDictionary(
//            securityContextType => securityContextType,
//            securityContextType =>
//            {
//                var securityContextRestrictionFilterInfo = filterInfoDict.GetValueOrDefault(securityContextType);

//                var baseIdents = rawRestrictions.GetValueOrDefault(realTypeResolver.Resolve(securityContextType), Array.Empty<TSecurityContextObjectIdent>());

//                if (securityContextRestrictionFilterInfo == null)
//                {
//                    return baseIdents;
//                }
//                else
//                {
//                    return this.ApplySecurityContextFilter(baseIdents, securityContextRestrictionFilterInfo);
//                }

//            });
//    }

//    private TSecurityContextObjectIdent[] ApplySecurityContextFilter(Array securityContextIdents, SecurityContextRestrictionFilterInfo restrictionFilterInfo)
//    {
//        return new Func<TSecurityContextObjectIdent[], SecurityContextRestrictionFilterInfo<ISecurityContext>, IReadOnlyList<TSecurityContextObjectIdent>>(
//                this.ApplySecurityContextFilter)
//            .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
//            .Invoke<TSecurityContextObjectIdent[]>(this, securityContextIdents, restrictionFilterInfo);
//    }

//    private TSecurityContextObjectIdent[] ApplySecurityContextFilter<TSecurityContext>(IReadOnlyList<TSecurityContextObjectIdent> baseSecurityContextIdents,
//        SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
//        where TSecurityContext : class, ISecurityContext
//    {
//        var identityInfo = identityInfoSource.GetIdentityInfo<TSecurityContext, TSecurityContextObjectIdent>();

//        var filteredSecurityContextQueryable = securityContextSource.GetQueryable(restrictionFilterInfo).Select(identityInfo.Id.Path);

//        if (baseSecurityContextIdents.Any())
//        {
//            return filteredSecurityContextQueryable.Where(securityContextId => baseSecurityContextIdents.Contains(securityContextId)).ToArray();
//        }
//        else
//        {
//            return filteredSecurityContextQueryable.ToArray();
//        }
//    }
//}
