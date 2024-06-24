#nullable enable

using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityPathRestrictionService(SecurityPathRestrictionServiceSettings? settings = null) : ISecurityPathRestrictionService
{
    public SecurityPath<TDomainObject> ApplyRestriction<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction)
    {
        var visitedSecurityPath = this.VisitSecurityContexts(securityPath, restriction);

        var result = restriction.Conditions.Aggregate(visitedSecurityPath, this.TryAddCondition);

        return result;
    }

    private SecurityPath<TDomainObject> VisitSecurityContexts<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction)
    {
        if (restriction.SecurityContexts == null)
        {
            return securityPath;
        }
        else
        {
            if (settings?.ValidateSecurityPath == true)
            {
                var usedTypes = securityPath.GetUsedTypes().ToList();

                var invalidTypes = restriction.SecurityContexts.Except(usedTypes).ToList();

                if (invalidTypes.Any())
                {
                    throw new Exception($"Can't apply restriction. Invalid types: {invalidTypes.Join(", ", t => t.Name)}");
                }
            }

            return this.Visit(securityPath, [.. restriction.SecurityContexts]);
        }
    }

    private SecurityPath<TDomainObject> TryAddCondition<TDomainObject>(SecurityPath<TDomainObject> securityPath, LambdaExpression condition)
    {
        var conditionType = condition.Parameters.Single().Type;

        if (condition is Expression<Func<TDomainObject, bool>> typedCondition)
        {
            return securityPath.And(typedCondition);
        }
        else if (conditionType.IsAssignableFrom(typeof(TDomainObject)))
        {
            return new Func<SecurityPath<TDomainObject>, Expression<Func<TDomainObject, bool>>, SecurityPath<TDomainObject>>(
                       this.AddCondition)
                   .CreateGenericMethod(typeof(TDomainObject), conditionType)
                   .Invoke<SecurityPath<TDomainObject>>(this, securityPath, condition);
        }
        else
        {
            return securityPath;
        }
    }

    private SecurityPath<TDomainObject> AddCondition<TDomainObject, TConditionSource>(
        SecurityPath<TDomainObject> securityPath,
        Expression<Func<TConditionSource, bool>> condition)
        where TDomainObject : TConditionSource
    {
        var castedExpr = condition.OverrideInput((TDomainObject source) => source);

        return securityPath.And(castedExpr);
    }

    private SecurityPath<TDomainObject> Visit<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        HashSet<Type> allowedSecurityContexts)
    {
        var pathType = securityPath.GetType();

        if (securityPath is SecurityPath<TDomainObject>.ConditionPath)
        {
            return securityPath;
        }
        else if (pathType.BaseType.Maybe(baseType => baseType.IsGenericTypeImplementation(typeof(SecurityPath<>))))
        {
            var singleSecurityContextType = pathType.GetGenericArguments().ToArray()[1];

            if (allowedSecurityContexts.Contains(singleSecurityContextType))
            {
                return securityPath;
            }
            else
            {
                return SecurityPath<TDomainObject>.Empty;
            }
        }
        else if (pathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            var func =
                new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, HashSet<Type>, SecurityPath<TDomainObject>>(
                    this.VisitNestedSecurityContexts);

            var args = pathType.GetGenericArguments().ToArray();

            var method = func.Method.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke<SecurityPath<TDomainObject>>(this, securityPath, allowedSecurityContexts);
        }
        else if (securityPath is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            var visitedLeft = this.Visit(andSecurityPath.Left, allowedSecurityContexts);

            var visitedRight = this.Visit(andSecurityPath.Right, allowedSecurityContexts);

            if (visitedLeft == SecurityPath<TDomainObject>.Empty)
            {
                return visitedRight;
            }
            else if (visitedRight == SecurityPath<TDomainObject>.Empty)
            {
                return visitedLeft;
            }
            else if (visitedLeft == andSecurityPath.Left && visitedRight == andSecurityPath.Right)
            {
                return andSecurityPath;
            }
            else
            {
                return visitedLeft.And(visitedRight);
            }
        }
        else if (securityPath is SecurityPath<TDomainObject>.OrSecurityPath orSecurityPath)
        {
            var visitedLeft = this.Visit(orSecurityPath.Left, allowedSecurityContexts);

            var visitedRight = this.Visit(orSecurityPath.Right, allowedSecurityContexts);

            if (visitedLeft == SecurityPath<TDomainObject>.Empty)
            {
                return visitedLeft;
            }
            else if (visitedRight == SecurityPath<TDomainObject>.Empty)
            {
                return visitedRight;
            }
            else if (visitedLeft == orSecurityPath.Left && visitedRight == orSecurityPath.Right)
            {
                return orSecurityPath;
            }
            else
            {
                return visitedLeft.Or(visitedRight);
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(securityPath));
        }
    }

    private SecurityPath<TDomainObject> VisitNestedSecurityContexts<TDomainObject, TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
        HashSet<Type> allowedSecurityContexts)
    {
        var visitedNestedSecurityPath = this.Visit(securityPath.NestedSecurityPath, allowedSecurityContexts);

        if (visitedNestedSecurityPath == securityPath.NestedSecurityPath)
        {
            return securityPath;
        }
        else
        {
            return securityPath with { NestedSecurityPath = visitedNestedSecurityPath };
        }
    }
}
