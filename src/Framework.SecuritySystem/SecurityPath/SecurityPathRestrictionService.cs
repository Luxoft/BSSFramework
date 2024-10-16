using System.Linq.Expressions;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class SecurityPathRestrictionService(IServiceProvider serviceProvider)
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
        if (restriction.SecurityContextRestrictions == null)
        {
            return securityPath;
        }
        else
        {
            return this.Visit(securityPath, restriction.SecurityContextRestrictions);
        }
    }

    private SecurityPath<TDomainObject> TryAddConditionFactory<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        Type conditionFactoryType)
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
        RelativeConditionInfo conditionInfo)
    {
        var factoryType = typeof(RelativeConditionFactory<,>).MakeGenericType(
            typeof(TDomainObject),
            conditionInfo.RelativeDomainObjectType);

        var untypedConditionFactory = ActivatorUtilities.CreateInstance(serviceProvider, factoryType, conditionInfo);

        var conditionFactory = (IFactory<Expression<Func<TDomainObject, bool>>>?)untypedConditionFactory;

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

    private SecurityPath<TDomainObject> Visit<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    {
        var pathType = securityPath.GetType();

        if (securityPath is SecurityPath<TDomainObject>.ConditionPath)
        {
            return securityPath;
        }
        else if (securityPath is IContextSecurityPath contextSecurityPath)
        {
            var containsKey = securityContextRestrictions.Any(
                                  restriction => restriction.Type == contextSecurityPath.SecurityContextType
                                                 && restriction.Key == contextSecurityPath.Key);

            return containsKey ? securityPath : SecurityPath<TDomainObject>.Empty;
        }
        else if (pathType.IsGenericTypeImplementation(typeof(SecurityPath<>.NestedManySecurityPath<>)))
        {
            var func =
                new Func<SecurityPath<TDomainObject>.NestedManySecurityPath<TDomainObject>, IReadOnlyList<SecurityContextRestriction>,
                    SecurityPath<TDomainObject>>(
                    this.VisitNestedSecurityContexts);

            var args = pathType.GetGenericArguments().ToArray();

            var method = func.Method.GetGenericMethodDefinition().MakeGenericMethod(args);

            return method.Invoke<SecurityPath<TDomainObject>>(this, securityPath, securityContextRestrictions);
        }
        else if (securityPath is SecurityPath<TDomainObject>.AndSecurityPath andSecurityPath)
        {
            var visitedLeft = this.Visit(andSecurityPath.Left, securityContextRestrictions);

            var visitedRight = this.Visit(andSecurityPath.Right, securityContextRestrictions);

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
            var visitedLeft = this.Visit(orSecurityPath.Left, securityContextRestrictions);

            var visitedRight = this.Visit(orSecurityPath.Right, securityContextRestrictions);

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
        IReadOnlyList<SecurityContextRestriction> securityContextRestrictions)
    {
        var visitedNestedSecurityPath = this.Visit(securityPath.NestedSecurityPath, securityContextRestrictions);

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
