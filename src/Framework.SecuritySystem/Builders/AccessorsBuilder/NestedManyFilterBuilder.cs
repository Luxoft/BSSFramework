using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class NestedManyFilterBuilder<TPermission, TDomainObject, TNestedObject>(
    AccessorsFilterBuilderFactory<TPermission, TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath) : AccessorsFilterBuilder<TPermission, TDomainObject>
{
    private AccessorsFilterBuilder<TPermission, TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath);

    public override Expression<Func<TPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType) =>
        securityPath.NestedObjectsPath.Eval(domainObject, LambdaCompileCache)
                    .BuildOr(item => this.NestedBuilder.GetAccessorsFilter(item, expandType));

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}
