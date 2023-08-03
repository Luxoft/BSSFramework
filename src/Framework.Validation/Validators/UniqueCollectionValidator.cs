using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.Validation;

public class UniqueCollectionValidator : IDynamicPropertyValidator
{
    private readonly string _groupKey;


    public UniqueCollectionValidator(string groupKey)
    {
        this._groupKey = groupKey;
    }


    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var elementType = property.PropertyType.GetCollectionElementType();

        var uniProperties = property.GetUniqueElementPropeties(this._groupKey, true);

        var groupElementType = typeof(Tuple<>).Assembly
                                              .GetType(typeof(Tuple<>).FullName.SkipLast("1", true) + uniProperties.Length, true)
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

        var internalPropertyValidatorType = typeof(UniqueCollectionValidator<,,,>).MakeGenericType(property.ReflectedType, property.PropertyType, elementType, groupElementType);

        var internalPropertyValidatorTypeCtor = internalPropertyValidatorType.GetConstructors().Single();

        var internalInstance = internalPropertyValidatorTypeCtor.Invoke(new object[] { getGroupElementExpr, uniProperties.GetUniqueElementString(false) });

        return (IPropertyValidator)internalInstance;
    }
}

public class UniqueCollectionValidator<TSource, TProperty, TElement, TGroupElement> : IPropertyValidator<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
{
    private readonly Func<TElement, TGroupElement> _getGroupElement;
    private readonly string _uniqueElementString;

    public UniqueCollectionValidator(Expression<Func<TElement, TGroupElement>> getGroupElement, string uniqueElementString)
    {
        if (getGroupElement == null) throw new ArgumentNullException(nameof(getGroupElement));
        if (uniqueElementString == null) throw new ArgumentNullException(nameof(uniqueElementString));

        this._getGroupElement = getGroupElement.Compile(CompileCache);
        this._uniqueElementString = uniqueElementString;
    }

    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
    {
        var duplicates = context.Value.GroupBy(this._getGroupElement).Where(g => g.Count() > 1).ToArray();

        return ValidationResult.FromCondition(

                                              !duplicates.Any(),

                                              () => $"{context.GetPropertyName()}{(context.GetSource() as IVisualIdentityObject).Maybe(x => " (" + x.Name + ")")} error. Duplicate fields ({this._uniqueElementString}) combination: {duplicates.Join(", ", d => d.Key)}");
    }

    private static readonly LambdaCompileCache CompileCache = new LambdaCompileCache();
}
