using System.Reflection;

using CommonFramework;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

public class ValidationMap(IServiceProvider serviceProvider) : ValidationMapBase(serviceProvider)
{
    protected override IClassValidationMap<TSource> GetInternalClassMap<TSource>()
    {
        return new ClassValidationMap<TSource>(this.GetPropertyMaps<TSource>(), this.GetClassValidators<TSource>());
    }

    private IEnumerable<IPropertyValidationMap<TSource>> GetPropertyMaps<TSource>()
    {
        return from property in typeof(TSource).GetValidationProperties()

               let getPropertyMapMethod = new Func<PropertyInfo, PropertyValidationMap<TSource, object>>(this.GetPropertyMap<TSource, object>)
                       .CreateGenericMethod(typeof(TSource), property.PropertyType)

               select getPropertyMapMethod.Invoke<IPropertyValidationMap<TSource>>(this, property);
    }

    private PropertyValidationMap<TSource, TProperty> GetPropertyMap<TSource, TProperty>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var collectionElementType = property.PropertyType.GetCollectionElementType();

        if (collectionElementType == null)
        {
            return this.GetSinglePropertyMap<TSource, TProperty>(property);
        }
        else
        {
            var func = new Func<PropertyInfo, CollectionPropertyValidationMap<object, IEnumerable<object>, object>>(this.GetCollectionPropertyMap<object, IEnumerable<object>, object>);

            return func.CreateGenericMethod(typeof(TSource), typeof(TProperty), collectionElementType).Invoke<PropertyValidationMap<TSource, TProperty>>(this, property);
        }
    }

    protected virtual bool HasDeepValidation(PropertyInfo property)
    {
        return property.HasDeepValidation();
    }

    protected virtual SinglePropertyValidationMap<TSource, TProperty> GetSinglePropertyMap<TSource, TProperty>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return new SinglePropertyValidationMap<TSource, TProperty>(
                                                                   property,
                                                                   this.GetClassMap<TSource>(),
                                                                   this.GetOperationContextPropertyValidators<TSource, TProperty>(property).Pipe(this.HasDeepValidation(property), val => val.Concat(new[] { new DeepSingleValidator<TSource, TProperty>() })),
                                                                   this.GetClassMap<TProperty>());
    }

    protected virtual CollectionPropertyValidationMap<TSource, TProperty, TElement> GetCollectionPropertyMap<TSource, TProperty, TElement>(PropertyInfo property)
            where TProperty : IEnumerable<TElement>
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return new CollectionPropertyValidationMap<TSource, TProperty, TElement>(
                                                                                 property,
                                                                                 this.GetClassMap<TSource>(),
                                                                                 this.GetOperationContextPropertyValidators<TSource, TProperty>(property).Pipe(this.HasDeepValidation(property), val => val.Concat(new[] { new DeepCollectionValidator<TSource, TProperty, TElement>() })),
                                                                                 this.GetClassMap<TElement>());
    }

    private IEnumerable<IPropertyValidator<TSource, TProperty>> GetOperationContextPropertyValidators<TSource, TProperty>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var aggregateValidators = this.GetPropertyDynamicClassValidators<TSource, TProperty>(property);

        var validators = this.GetBasePropertyValidators<TSource, TProperty>(property);

        return aggregateValidators.Concat(validators);
    }


    private IEnumerable<IClassValidator<TSource>> GetClassValidators<TSource>()
    {
        var classValidators = from pair in this.GetClassValidatorDict<TSource, IClassValidator>()

                              where pair.Key is IClassValidator<TSource> || pair.Key is IDynamicClassValidator

                              let baseClassValidator = pair.Key.GetLastClassValidator(typeof(TSource), this.ServiceProvider)

                              let classValidator = (IClassValidator<TSource>)baseClassValidator

                              select classValidator.TryApplyValidationData(pair.Value);


        var selfClassValidator = typeof(IClassValidator<TSource>).IsAssignableFrom(typeof(TSource))
                                         ? (IClassValidator<TSource>)ActivatorUtilities.CreateInstance(this.ServiceProvider, typeof(SelfClassValidator<>).MakeGenericType(typeof(TSource)))
                                         : null;

        return classValidators.Concat(selfClassValidator.MaybeYield());
    }


    private IEnumerable<IPropertyValidator<TSource, TProperty>> GetPropertyDynamicClassValidators<TSource, TProperty>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.GetClassValidatorDict<TSource, IManyPropertyDynamicClassValidator>().Select(pair =>
        {
            var basePropertyValidator = pair.Key.GetLastPropertyValidator(property, this.ServiceProvider);

            try
            {
                return basePropertyValidator.Maybe(val => val.TryUnbox<TSource, TProperty>().TryApplyValidationData(pair.Value));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Can't apply validator \"{basePropertyValidator?.GetType().Name}\" to property \"{property.Name}\" of type \"{property.DeclaringType}\"", ex);
            }
        }).Where(val => val != null).Select(v => v!);
    }

    private IEnumerable<IPropertyValidator<TSource, TProperty>> GetBasePropertyValidators<TSource, TProperty>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.GetPropertyValidatorDict(property).Select(pair =>
                                                              {
                                                                  var basePropertyValidator = pair.Key.GetLastPropertyValidator(property, this.ServiceProvider);

                                                                  try
                                                                  {
                                                                      return basePropertyValidator.TryUnbox<TSource, TProperty>().TryApplyValidationData(pair.Value);
                                                                  }
                                                                  catch (Exception ex)
                                                                  {
                                                                      throw new InvalidCastException($"Can't apply validator \"{basePropertyValidator.GetType().Name}\" to property \"{property.Name}\" of type \"{property.DeclaringType}\"", ex);
                                                                  }
                                                              });
    }

    protected virtual IEnumerable<KeyValuePair<IClassValidator, IValidationData>> GetClassValidatorDict<TSource>()
    {
        return from attribute in typeof(TSource).GetCustomAttributes<ClassValidatorAttribute>()

               select attribute.CreateValidator(this.ServiceProvider).ToKeyValuePair((IValidationData)attribute);
    }

    private IEnumerable<KeyValuePair<TFilterValidator, IValidationData>> GetClassValidatorDict<TSource, TFilterValidator>()
            where TFilterValidator : class
    {
        return from pair in this.GetClassValidatorDict<TSource>()

               let classValidator = pair.Key as TFilterValidator

               where classValidator != null

               select classValidator.ToKeyValuePair(pair.Value);
    }


    protected virtual IEnumerable<PropertyValidatorAttribute> GetPropertyValidatorAttributes(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        var restrictionAttributes = property.TryGetRestrictionValidatorAttributes();

        return property.GetCustomAttributes<PropertyValidatorAttribute>().Concat(restrictionAttributes);
    }

    protected virtual IEnumerable<KeyValuePair<IPropertyValidator, IValidationData>> GetPropertyValidatorDict(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return from attribute in this.GetPropertyValidatorAttributes(property)

               select attribute.CreateValidator(this.ServiceProvider).ToKeyValuePair((IValidationData)attribute);
    }


    public static readonly ValidationMap Default = new ValidationMap(new ServiceCollection().BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));
}
