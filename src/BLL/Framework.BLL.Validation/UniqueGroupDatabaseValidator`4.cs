using System.Linq.Expressions;

using CommonFramework;

using Framework.Application.Domain;
using Framework.Validation;
using Framework.Validation.Validators;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL.Validation;

/// <summary>
/// /// <summary>
/// Класс для проверки уникальности полей доменного объекта через запрос к БД.
/// </summary>
/// <seealso cref="IDynamicClassValidator" />
/// </summary>
/// <seealso cref="IClassValidator" />
public class UniqueGroupDatabaseValidator<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(
    Func<TDomainObject, Expression<Func<TDomainObject, bool>>> getFilterExpression,
    Func<TDomainObject, object[]> getPropertyValues,
    string propertyName)
    : IClassValidator<TDomainObject>
    where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
{
    public ValidationResult GetValidationResult(IClassValidationContext<TDomainObject> validationContext)
    {
        var context = validationContext.ServiceProvider.GetRequiredService<TBLLContext>();

        var query = context.Logics.Default.Create<TDomainObject>().GetUnsecureQueryable();

        var filter = getFilterExpression(validationContext.Source);

        var hasDuplicate = query.Any(filter);

        return ValidationResult.FromCondition(
            !hasDuplicate,
            () => $"{typeof(TDomainObject).Name} with same {propertyName} ({getPropertyValues(validationContext.Source).Join(", ")}) already exists");
    }
}
