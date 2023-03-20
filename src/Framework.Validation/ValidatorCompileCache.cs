using System;
using System.Linq;

using Framework.Core;

namespace Framework.Validation;

public class ValidatorCompileCache
{
    private readonly IValidationMap validationMap;

    private readonly LambdaCompileCache lambdaCompileCache = new LambdaCompileCache();

    private readonly IDictionaryCache<Type, Delegate> validateFuncCache;

    public ValidatorCompileCache(IValidationMap validationMap)
    {
        if (validationMap == null) throw new ArgumentNullException(nameof(validationMap));

        this.validationMap = validationMap;

        this.validateFuncCache = new DictionaryCache<Type, Delegate>(type =>
                                                                     {
                                                                         var classMap = this.validationMap.GetClassMap(type);

                                                                         Func<ClassValidationMap<object>, Func<IValidationContextBase<object>, ValidationResult>> func = this.GetClassValidationFunc;

                                                                         return func.CreateGenericMethod(type).Invoke<Delegate>(this, classMap);
                                                                     }).WithLock();
    }

    /// <summary>
    /// Поддержка null-объектов (будет возвращаться Success)
    /// </summary>
    public virtual bool AllowNullSource { get; } = true;

    /// <summary>
    /// Получение результата валидации
    /// </summary>
    /// <typeparam name="TSource">Тип объекта</typeparam>
    /// <param name="context">Контекст</param>
    /// <returns></returns>
    public ValidationResult GetValidationResult<TSource>(IValidationContextBase<TSource> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        if (this.AllowNullSource && context.Source == null)
        {
            return ValidationResult.Success;
        }
        else
        {
            var del = this.validateFuncCache[typeof(TSource)];

            var typedDel = (Func<IValidationContextBase<TSource>, ValidationResult>)del;

            return typedDel(context);
        }
    }

    private IClassValidationContext<TSource> CreateClassValidationContext<TSource>(IValidationContextBase<TSource> context, IValidationState ownerState, IClassValidationMap<TSource> map)
    {
        return new ClassValidationContext<TSource>(context.Validator, context.OperationContext, context.Source, ownerState, map, this.validationMap.ExtendedValidationData.TryAdd(context.Validator as IExtendedValidationDataContainer));
    }

    private IPropertyValidationContext<TSource, TProperty> CreatePropertyValidationContext<TSource, TProperty>(IValidationContextBase<TSource> context, IValidationState ownerState, IPropertyValidationMap<TSource, TProperty> map, TProperty value)
    {
        return new PropertyValidationContext<TSource, TProperty>(context.Validator, context.OperationContext, context.Source, ownerState, map, this.validationMap.ExtendedValidationData.TryAdd(context.Validator as IExtendedValidationDataContainer), value);
    }

    private Func<IValidationContextBase<TSource>, ValidationResult> GetClassValidationFunc<TSource>(IClassValidationMap<TSource> classMap)
    {
        if (classMap == null) throw new ArgumentNullException(nameof(classMap));

        var classValidateFunc = classMap.Validators.Any()

                                        ? FuncHelper.Create((IValidationContextBase<TSource> context) =>
                                                            {
                                                                if (this.AllowNullSource && context.Source == null)
                                                                {
                                                                    return ValidationResult.Success;
                                                                }
                                                                else
                                                                {
                                                                    var classValidationContext = this.CreateClassValidationContext(context, context.ParentState, classMap);

                                                                    return classMap.Validators.Sum(classValidator => classValidator.GetValidationResult(classValidationContext));
                                                                }
                                                            })

                                        : _ => ValidationResult.Success;

        var getValidationFuncByPropMethod = new Func<IPropertyValidationMap<TSource, object>, Func<IValidationContextBase<TSource>, ValidationResult>>(this.GetPropertyValidationFunc).Method.GetGenericMethodDefinition();

        var propertiesValidateFuncs = from propertyMap in classMap.PropertyMaps

                                      let getMethod = getValidationFuncByPropMethod.MakeGenericMethod(typeof(TSource), propertyMap.Property.PropertyType)

                                      select (Func<IValidationContextBase<TSource>, ValidationResult>)getMethod.Invoke(this, new object[] { propertyMap });

        return new[] { classValidateFunc }.Concat(propertiesValidateFuncs).Sum();
    }

    private Func<IValidationContextBase<TSource>, ValidationResult> GetPropertyValidationFunc<TSource, TProperty>(IPropertyValidationMap<TSource, TProperty> propertyMap)
    {
        if (propertyMap == null) throw new ArgumentNullException(nameof(propertyMap));

        var getPropertyValueFunc = propertyMap.Property.ToLambdaExpression<TSource, TProperty>().Compile(this.lambdaCompileCache);

        return propertyMap.Validators.Any()

                       ? FuncHelper.Create((IValidationContextBase<TSource> context) =>
                                           {
                                               if (this.AllowNullSource && context.Source == null)
                                               {
                                                   return ValidationResult.Success;
                                               }
                                               else
                                               {
                                                   var propertyValidationContext = this.CreatePropertyValidationContext(context, context.ParentState, propertyMap, getPropertyValueFunc(context.Source));

                                                   return propertyMap.Validators.Sum(propertyValidator => propertyValidator.GetValidationResult(propertyValidationContext));
                                               }
                                           })
                       : _ => ValidationResult.Success;
    }
}

public static class ValidatorCompileCacheExtensions
{
    public static ValidatorCompileCache ToCompileCache(this IValidationMap validationMap)
    {
        if (validationMap == null) throw new ArgumentNullException(nameof(validationMap));

        return new ValidatorCompileCache(validationMap);
    }
}
