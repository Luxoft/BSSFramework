using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Application.Domain;
using Framework.Core;
using Framework.Restriction;
using Framework.Validation.Validators;

namespace Framework.Validation;

public class UniqueCollectionValidator(string groupKey) : IDynamicPropertyValidator
{
    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var elementType = property.PropertyType.GetCollectionElementType()!;

        var uniProperties = property.GetUniqueElementProperties(groupKey, true);

        var groupElementType = typeof(Tuple<>).Assembly
                                              .GetType(typeof(Tuple<>).FullName!.SkipLast("1", true) + uniProperties.Length, true)!
                                              .MakeGenericType(uniProperties.ToArray(p => p.PropertyType));


        var elementParameter = Expression.Parameter(elementType);

        Expression<Func<string, string>> normalizeString = str => str.TrimNull().ToLower();

        var members = uniProperties.Select(prop =>
                                           {
                                               var basePropExpr = Expression.Property(elementParameter, prop);

                                               if(prop.PropertyType == typeof(string))
                                               {
                                                   return normalizeString.GetBodyWithOverrideParameters(basePropExpr);
                                               }
                                               else
                                               {
                                                   return basePropExpr;
                                               }
                                           });


        var newExpr = Expression.New(groupElementType.GetConstructors().Single(), members);

        var getGroupElementExpr = Expression.Lambda(newExpr, elementParameter);

        var internalPropertyValidatorType = typeof(UniqueCollectionValidator<,,,>).MakeGenericType(property.ReflectedType!, property.PropertyType, elementType, groupElementType);

        var internalPropertyValidatorTypeCtor = internalPropertyValidatorType.GetConstructors().Single();

        var internalInstance = internalPropertyValidatorTypeCtor.Invoke([getGroupElementExpr, uniProperties.GetUniqueElementString(false)]);

        return (IPropertyValidator)internalInstance;
    }
}

public class UniqueCollectionValidator<TSource, TProperty, TElement, TGroupElement> : IPropertyValidator<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
{
    private readonly Func<TElement, TGroupElement> getGroupElement;
    private readonly string uniqueElementString;

    public UniqueCollectionValidator(Expression<Func<TElement, TGroupElement>> getGroupElement, string uniqueElementString)
    {
        if (getGroupElement == null) throw new ArgumentNullException(nameof(getGroupElement));
        if (uniqueElementString == null) throw new ArgumentNullException(nameof(uniqueElementString));

        this.getGroupElement = getGroupElement.Compile();
        this.uniqueElementString = uniqueElementString;
    }

    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
    {
        var duplicates = context.Value.GroupBy(this.getGroupElement).Where(g => g.Count() > 1).ToArray();

        return ValidationResult.FromCondition(

                                              !duplicates.Any(),

                                              () => $"{context.GetPropertyName()}{(context.GetSource() as IVisualIdentityObject).Maybe(x => " (" + x.Name + ")")} error. Duplicate fields ({this.uniqueElementString}) combination: {duplicates.Join(", ", d => d.Key)}");
    }
}
