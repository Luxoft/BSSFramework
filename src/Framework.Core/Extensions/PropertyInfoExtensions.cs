﻿using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public static class PropertyInfoExtensions
{
    public static bool IsHierarchical(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.PropertyType.GetCollectionElementType() == propertyInfo.DeclaringType;
    }

    public static bool IsAbstract(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return new[]
               {
                       propertyInfo.GetGetMethod(),
                       propertyInfo.GetGetMethod(true),
                       propertyInfo.GetSetMethod(),
                       propertyInfo.GetSetMethod(true),
               }.Where(method => method != null).Any(method => method.IsAbstract);
    }

    public static bool HasGetMethod(this PropertyInfo propertyInfo, bool nonPublic = false)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetGetMethod(nonPublic) != null;
    }

    /// <summary>
    /// Проверка наличия базового свойства
    /// </summary>
    /// <param name="propertyInfo">Проверяемое свойство</param>
    /// <param name="nonPublic">Маркер публичности</param>
    /// <returns></returns>
    public static bool HasBaseProp(this PropertyInfo propertyInfo, bool nonPublic = false)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetGetMethod(nonPublic)?.GetBaseDefinition() != null;
    }

    public static bool HasSetMethod(this PropertyInfo propertyInfo, bool nonPublic = false)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetSetMethod(nonPublic) != null;
    }

    public static bool HasFamilyGetMethod(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasGetMethod()
               || propertyInfo.GetGetMethod(true).Maybe(method => method.Attributes.HasFlag(MethodAttributes.Family));
    }

    public static bool HasFamilySetMethod(this PropertyInfo propertyInfo)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasSetMethod()
               || propertyInfo.GetSetMethod(true).Maybe(method => method.Attributes.HasFlag(MethodAttributes.Family));
    }

    /// <summary>
    /// Получение приватного филда по свойству
    /// </summary>
    /// <param name="propertyInfo">Свойство</param>
    /// <param name="preFieldName">Кастомное имя филда</param>
    /// <returns></returns>
    public static FieldInfo GetPrivateField(this PropertyInfo propertyInfo, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetPrivateFieldInternal(propertyInfo.DeclaringType, preFieldName);
    }

    public static bool HasPrivateField(this PropertyInfo propertyInfo, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasPrivateFieldInternal(preFieldName);
    }

    public static bool HasPrivateField(this PropertyInfo propertyInfo, bool withAutoGenerated)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasPrivateFieldInternal()
               || (withAutoGenerated && propertyInfo.HasPrivateField($"<{propertyInfo.Name}>k__BackingField"));
    }

    private static bool HasPrivateFieldInternal(this PropertyInfo propertyInfo, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasPrivateField(propertyInfo.DeclaringType, preFieldName);
    }

    public static bool HasPrivateField(this PropertyInfo propertyInfo, Type declaringType, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.HasPrivateFieldInternal(declaringType, preFieldName)
                // || (preFieldName == null && propertyInfo.HasPrivateFieldInternal("_" + propertyInfo.Name.ToStartLowerCase()))
                ;
    }


    private static bool HasPrivateFieldInternal(this PropertyInfo propertyInfo, Type declaringType, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        return propertyInfo.GetPrivateFieldInternal(declaringType, preFieldName) != null;
    }

    private static FieldInfo GetPrivateFieldInternal(this PropertyInfo propertyInfo, Type declaringType, string preFieldName = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var fieldName = preFieldName ?? propertyInfo.Name.ToStartLowerCase();

        var request = from type in declaringType.GetAllElements(t => t.BaseType)

                      let field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)

                      where field != null && field.DeclaringType == type && propertyInfo.PropertyType.IsAssignableFrom(field.FieldType)

                      select field;

        return request.SingleOrDefault();
    }

    public static LambdaExpression ToSetLambdaExpression(this PropertyInfo property, Type sourceType = null)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return PropertyLambdaCache.SetLambdaCache.GetValue(property, sourceType ?? property.ReflectedType);
    }

    public static Expression<Action<TSource, TProperty>> ToSetLambdaExpression<TSource, TProperty>(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return PropertyLambdaCache<TSource, TProperty>.SetLambdaCache[property];
    }

    public static Expression<Action<TSource, TProperty>> ToSetLambdaExpression<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expr)
    {
        return expr.GetProperty().ToSetLambdaExpression<TSource, TProperty>();
    }

    public static LambdaExpression ToLambdaExpression(this PropertyInfo property, Type sourceType = null)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return PropertyLambdaCache.LambdaCache.GetValue(property, sourceType ?? property.ReflectedType);
    }

    public static Expression<Func<TSource, TProperty>> ToLambdaExpression<TSource, TProperty>(this PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return PropertyLambdaCache<TSource, TProperty>.LambdaCache[property];
    }

    public static TProperty GetValue<TSource, TProperty>(this PropertyInfo property, TSource source)
    {
        var func = PropertyLambdaCache<TSource, TProperty>.FuncCache[property];

        return func(source);
    }

    public static void SetValue<TSource, TProperty>(this PropertyInfo property, TSource source, TProperty value)
    {
        var action = property.GetSetValueAction<TSource, TProperty>();

        action(source, value);
    }

    public static Action<TSource, TProperty> GetSetValueAction<TSource, TProperty>(this PropertyInfo property)
    {
        return PropertyLambdaCache<TSource, TProperty>.SetActionCache[property];
    }

    private static class PropertyLambdaCache
    {
        public static readonly IDictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression> LambdaCache = new DictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression>(tuple =>
        {
            var property = tuple.Item1;
            var propertySource = tuple.Item2;

            var parameter = Expression.Parameter(propertySource);

            return Expression.Lambda(Expression.Property(parameter, property), parameter);
        }).WithLock();

        public static readonly IDictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression> SetLambdaCache = new DictionaryCache<Tuple<PropertyInfo, Type>, LambdaExpression>(tuple =>
        {
            var property = tuple.Item1;
            var propertySource = tuple.Item2;

            var sourceParameter = Expression.Parameter(propertySource);
            var valueParameter = Expression.Parameter(property.PropertyType);

            return Expression.Lambda(Expression.Call(sourceParameter, property.GetSetMethod() ?? property.GetSetMethod(true), valueParameter), sourceParameter, valueParameter);
        }).WithLock();
    }

    private static class PropertyLambdaCache<TSource, TProperty>
    {
        public static readonly IDictionaryCache<PropertyInfo, Expression<Func<TSource, TProperty>>> LambdaCache = new DictionaryCache<PropertyInfo, Expression<Func<TSource, TProperty>>>(property =>
                (Expression<Func<TSource, TProperty>>)property.ToLambdaExpression(typeof(TSource))).WithLock();

        public static readonly IDictionaryCache<PropertyInfo, Func<TSource, TProperty>> FuncCache = new DictionaryCache<PropertyInfo, Func<TSource, TProperty>>(property =>
                LambdaCache[property].Compile()).WithLock();


        public static readonly IDictionaryCache<PropertyInfo, Expression<Action<TSource, TProperty>>> SetLambdaCache = new DictionaryCache<PropertyInfo, Expression<Action<TSource, TProperty>>>(property =>
                (Expression<Action<TSource, TProperty>>)property.ToSetLambdaExpression(typeof(TSource))).WithLock();

        public static readonly IDictionaryCache<PropertyInfo, Action<TSource, TProperty>> SetActionCache = new DictionaryCache<PropertyInfo, Action<TSource, TProperty>>(property =>
                SetLambdaCache[property].Compile()).WithLock();
    }
}
