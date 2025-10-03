using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Persistent;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Класс для проверки уникальности полей доменного объекта через запрос к БД.
/// </summary>
/// <seealso cref="IDynamicClassValidator" />
public class UniqueGroupDatabaseValidator : IDynamicClassValidator
{
    private readonly string groupKey;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="UniqueGroupDatabaseValidator"/> class.
    /// </summary>
    /// <param name="groupKey">Ключ группы уникальности.</param>
    public UniqueGroupDatabaseValidator(string groupKey)
    {
        this.groupKey = groupKey;
    }

    /// <summary>
    /// Возвращает экземпляр валидатора..
    /// </summary>
    /// <param name="domainObjectType">Тип доменного объекта.</param>
    /// <param name="serviceProvider">Данные, необходимые для создания валидатора.</param>
    /// <returns>Экземпляр <see cref="IClassValidator"/>.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="domainObjectType"/>
    /// или
    /// <paramref name="serviceProvider"/> равен null.
    /// </exception>
    public IClassValidator GetValidator(Type domainObjectType, IServiceProvider serviceProvider)
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var contexTypeData = serviceProvider.GetRequiredService<BLLContextTypeData>();

        var internalValidatorType = typeof(UniqueGroupDatabaseValidator<,,,>)
                .MakeGenericType(
                                 contexTypeData.ContextType,
                                 contexTypeData.PersistentDomainObjectBaseType,
                                 domainObjectType,
                                 contexTypeData.IdentType);

        var uniProperties = domainObjectType.GetUniqueElementPropeties(this.groupKey, true);

        var propertyNames = uniProperties.GetUniqueElementString(false);

        var getFilterExpression = this.GetGetFilterExpression(
                                                              domainObjectType,
                                                              contexTypeData.IdentType,
                                                              uniProperties).Compile();

        var getPropertyValuesFunc = GetGetPropertyValuesExpression(domainObjectType, uniProperties).Compile();

        var validator = (IClassValidator)internalValidatorType.GetConstructors().Single().Invoke(
         new object[] { getFilterExpression, getPropertyValuesFunc, propertyNames });

        return validator;
    }

    private static LambdaExpression GetGetPropertyValuesExpression(
            Type domainObjectType,
            IEnumerable<PropertyInfo> properties)
    {
        var domainObjectParameter = Expression.Parameter(domainObjectType);

        var newValuesArrExpr = Expression.NewArrayInit(
                                                       typeof(object), properties.Select(
                                                        property => Expression.Property(domainObjectParameter, property).Pipe(propExpr =>
                                                                propExpr.Type.IsClass ? (Expression)propExpr : Expression.Convert(propExpr, typeof(object)))));

        return Expression.Lambda(newValuesArrExpr, domainObjectParameter);
    }

    private LambdaExpression GetGetFilterExpression(Type domainObjectType, Type identType, IEnumerable<PropertyInfo> properties)
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));
        if (identType == null) throw new ArgumentNullException(nameof(identType));
        if (properties == null) throw new ArgumentNullException(nameof(properties));

        var idProp = typeof(IIdentityObject<>).MakeGenericType(identType).GetProperties().Single();

        var sourceDomainObjectParameter = Expression.Parameter(domainObjectType);

        var filterDomainObjectParameter = Expression.Parameter(domainObjectType);

        Func<PropertyInfo, Func<Expression, Expression, Expression>, Expression> getBinaryExpr =
                (property, buildFunc) => buildFunc(
                                                   Expression.Property(sourceDomainObjectParameter, property),
                                                   Expression.Property(filterDomainObjectParameter, property));

        var duplicateFilter = properties.Aggregate(getBinaryExpr(idProp, Expression.NotEqual), (filter, property) =>
                                                           Expression.AndAlso(filter, getBinaryExpr(property, (e1, e2) => this.GetEqualityMethod(property, e1, e2))));

        var duplicateLambda = Expression.Lambda(duplicateFilter, filterDomainObjectParameter);

        return Expression.Lambda(Expression.Quote(duplicateLambda), sourceDomainObjectParameter);
    }

    private Expression GetEqualityMethod(PropertyInfo property, Expression e1, Expression e2)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var eqMethod = property.PropertyType.GetEqualityMethod(true);
        return Expression.Equal(e1, e2, property.PropertyType.IsNullable(), eqMethod);
    }
}
