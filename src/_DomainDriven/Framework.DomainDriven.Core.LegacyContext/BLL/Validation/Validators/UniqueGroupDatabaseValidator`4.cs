using System.Linq.Expressions;

using Framework.Persistent;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

using CommonFramework;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// /// <summary>
/// Класс для проверки уникальности полей доменного объекта через запрос к БД.
/// </summary>
/// <seealso cref="IDynamicClassValidator" />
/// </summary>
/// <seealso cref="IClassValidator{TDomainObject}" />
public class UniqueGroupDatabaseValidator<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent> : IClassValidator<TDomainObject>
        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    private readonly Func<TDomainObject, Expression<Func<TDomainObject, bool>>> getFilterExpression;
    private readonly Func<TDomainObject, object[]> getPropertyValues;
    private readonly string propertyName;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="UniqueGroupDatabaseValidator{TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent}"/>.
    /// </summary>
    /// <param name="getFilterExpression">Делегат, возвращающий фильтрующее выражение.</param>
    /// <param name="getPropertyValues">Делегат, возвращающий свойства доменного объекта.</param>
    /// <param name="propertyName">Имя свойства доменного объекта.</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="getFilterExpression"/>
    /// или
    /// <paramref name="getPropertyValues"/>
    /// или
    /// <paramref name="propertyName"/> равен null.
    /// </exception>
    public UniqueGroupDatabaseValidator(
            Func<TDomainObject, Expression<Func<TDomainObject, bool>>> getFilterExpression,
            Func<TDomainObject, object[]> getPropertyValues,
            string propertyName)
    {
        if (getFilterExpression == null) throw new ArgumentNullException(nameof(getFilterExpression));
        if (getPropertyValues == null) throw new ArgumentNullException(nameof(getPropertyValues));
        if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

        this.getFilterExpression = getFilterExpression;
        this.getPropertyValues = getPropertyValues;
        this.propertyName = propertyName;
    }

    /// <summary>
    /// Возвращает результат проверки.
    /// </summary>
    /// <param name="validationContext">Контекст проверки.</param>
    /// <returns>Экземпляр <see cref="ValidationResult"/>.</returns>
    public ValidationResult GetValidationResult(IClassValidationContext<TDomainObject> validationContext)
    {
        var context = validationContext.ServiceProvider.GetRequiredService<TBLLContext>();

        var query = context.Logics.Default.Create<TDomainObject>().GetUnsecureQueryable();

        var filter = this.getFilterExpression(validationContext.Source);

        var hasDuplicate = query.Any(filter);

        return ValidationResult.FromCondition(
                                              !hasDuplicate,
                                              () => $"{typeof(TDomainObject).Name} with same {this.propertyName} ({this.getPropertyValues(validationContext.Source).Join(", ")}) already exists");
    }
}
