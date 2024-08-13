using System.Linq.Expressions;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class SecurityPathRestrictionService(IServiceProvider serviceProvider, SecurityPathRestrictionServiceSettings? settings = null)
    : ISecurityPathRestrictionService
{
    public SecurityPath<TDomainObject> ApplyRestriction<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction)
    {
        var visitedSecurityPath = this.VisitSecurityContexts(securityPath, restriction);

        var addConditionFactoryResult = restriction.ConditionFactoryTypes.Aggregate(visitedSecurityPath, this.TryAddConditionFactory);

        var addRelativeConditionResult = restriction.RelativeConditions.Aggregate(addConditionFactoryResult, this.TryAddRelativeCondition);

        return addRelativeConditionResult;
    }

    private SecurityPath<TDomainObject> VisitSecurityContexts<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction)
    {
        if (restriction.SecurityContextTypes == null)
        {
            return securityPath;
        }
        else
        {
            if (settings?.ValidateSecurityPath == true)
            {
                var usedTypes = securityPath.GetUsedTypes().ToList();

                var invalidTypes = restriction.SecurityContextTypes.Except(usedTypes).ToList();

                if (invalidTypes.Any())
                {
                    throw new Exception($"Can't apply restriction. Invalid types: {invalidTypes.Join(", ", t => t.Name)}");
                }
            }

            return this.Visit(securityPath, [.. restriction.SecurityContextTypes]);
        }
    }

    private SecurityPath<TDomainObject> TryAddConditionFactory<TDomainObject>(SecurityPath<TDomainObject> securityPath, Type conditionFactoryType)
    {
        var conditionFactory =
            (IFactory<Expression<Func<TDomainObject, bool>>>?)serviceProvider.GetService(
                conditionFactoryType.MakeGenericType(typeof(TDomainObject)));

        var condition = conditionFactory?.Create();

        if (condition != null)
        {
            return securityPath.And(condition);
        }
        else
        {
            return securityPath;
        }
    }

    private SecurityPath<TDomainObject> TryAddRelativeCondition<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestrictionConditionInfo conditionInfo)
    {
        var untypedFactory = ActivatorUtilities.CreateInstance(
            serviceProvider,
            typeof(RelativeConditionFactory<,>).MakeGenericType(typeof(TDomainObject), conditionInfo.RelativeTargetDomainObjectType),
            conditionInfo);

        var factory = (IFactory<Expression<Func<TDomainObject, bool>>>)untypedFactory;

        return securityPath.And(factory.Create());
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
