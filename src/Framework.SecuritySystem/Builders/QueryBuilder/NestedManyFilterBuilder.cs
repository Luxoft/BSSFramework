﻿using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class NestedManyFilterBuilder<TPermission, TDomainObject, TNestedObject>(
    SecurityFilterBuilderFactory<TPermission, TNestedObject> nestedBuilderFactory,
    SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath) : SecurityFilterBuilder<TPermission, TDomainObject>
{
    private SecurityFilterBuilder<TPermission, TNestedObject> NestedBuilder { get; } = nestedBuilderFactory.CreateBuilder(securityPath.NestedSecurityPath);

    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
        HierarchicalExpandType expandType)
    {
        var baseFilter = this.NestedBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.Any:

                return (domainObject, permission) => !securityPath.NestedObjectsPath.Eval(domainObject).Any()

                                                     || securityPath.NestedObjectsPath.Eval(domainObject).Any(
                                                         nestedObject => baseFilter.Eval(nestedObject, permission));

            case ManySecurityPathMode.AnyStrictly:

                return (domainObject, permission) => securityPath.NestedObjectsPath.Eval(domainObject)
                                                         .Any(nestedObject => baseFilter.Eval(nestedObject, permission));

            default:

                throw new ArgumentOutOfRangeException("securityPath.Mode");
        }
    }
}
