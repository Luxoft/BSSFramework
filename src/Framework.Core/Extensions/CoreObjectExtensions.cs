using System;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core
{
    public static class CoreObjectExtensions
    {
        [Obsolete("v10 Use ?.")]
        public static int TryGetHashCode<T>(this T source)
            where T : class
        {
            return source.Maybe(v => v.GetHashCode());
        }

        private static readonly MethodInfo DefaultToStringMethod = new Func<string>(new object().ToString).Method;

        private static readonly IDictionaryCache<Type, bool> IsBaseToStringDict =
            new DictionaryCache<Type, bool>(
                    t => t.GetMethods()
                          .Where(m => m.DeclaringType == typeof(object))
                          .Select(m => m.GetBaseDefinition())
                          .Contains(DefaultToStringMethod))
                .WithLock();

        [Obsolete("v10 This method will be protected in future")]
        public static string ToFormattedString(this object source, string typeName = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var sourceType = source.GetType();
            var printTypeName = typeName ?? sourceType.Name;

            return IsBaseToStringDict[sourceType]
                       ? printTypeName
                       : $"{printTypeName} ({source})";
        }

        [Obsolete("v10 This method will be protected in future")]
        public static T TryExtractSource<T>(this T obj)
            where T : class
        {
            var innerObj = (obj as Lazy<T>).Maybe(lazy => lazy.Value)
                           ?? (obj as IContainer<T>).Maybe(container => container.Value);

            return innerObj.Maybe(v => v.TryExtractSource()) ?? obj;
        }

        [Obsolete("v10 This method will be protected in future")]
        public static ExpandoObject ToExpandoObject(this object source)
        {
            if (source == null)
            {
                return null;
            }

            var obj = new ExpandoObject();

            IDictionary<string, object> dict = obj;

            foreach (var prop in source.GetType().GetProperties())
            {
                var basePropValue = prop.GetValue(source, null);

                var propValue = prop.PropertyType.IsClass
                                && prop.PropertyType != typeof(string)
                                && !prop.PropertyType.IsArray
                                    ? basePropValue.ToExpandoObject()
                                    : prop.PropertyType.IsArray(t => !t.IsPrimitiveType())
                                        ? ((IEnumerable<object>)basePropValue).ToArray(item => item.ToExpandoObject())
                                        : basePropValue;

                dict.Add(prop.Name, propValue);
            }

            return obj;
        }

        /// <summary>
        ///Defines availability of different values in properties (identification of properties by name and type )
        /// </summary>
        /// <typeparam name="TArg1">The type of the arg1.</typeparam>
        /// <typeparam name="TArg2">The type of the arg2.</typeparam>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        [Obsolete("v10 This method will be protected in future")]
        public static bool HasDiffPropertyValues<TArg1, TArg2>(
            this TArg1 arg1,
            TArg2 arg2,
            params Expression<Func<TArg1, object>>[] ignoreProperties)
        {
            var ignorePropertiesDict = ignoreProperties.Select(z => z.ToPath()).ToHashSet();

            var arg1Properties = typeof(TArg1).GetProperties().Where(z => !ignorePropertiesDict.Contains(z.Name));
            var arg2Properties = typeof(TArg2).GetProperties().Where(z => !ignorePropertiesDict.Contains(z.Name));

            var pairs = arg1Properties.Join(
                                          arg2Properties,
                                          z => TupleStruct.Create(z.Name, z.PropertyType),
                                          z => TupleStruct.Create(z.Name, z.PropertyType),
                                          TupleStruct.Create)
                                      .ToList();

            return pairs.Any(
                z => !Equals(z.Item1.GetValue(arg1, new object[0]), z.Item2.GetValue(arg2, new object[0])));
        }

        /// <summary>
        /// Copies properties' values fufwrom one object to another (identification of properties by name and type)
        /// </summary>
        /// <typeparam name="TArg1"></typeparam>
        /// <typeparam name="TArg2"></typeparam>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        [Obsolete("v10 This method will be protected in future")]
        public static void CopyPropertiesFrom<TArg1, TArg2>(this TArg1 arg1, TArg2 arg2)
        {
            var arg1Properties = typeof(TArg1).GetProperties().Where(z => null != z.GetSetMethod());
            var arg2Properties = typeof(TArg2).GetProperties().Where(z => null != z.GetGetMethod());

            var pairs = arg1Properties.Join(
                                          arg2Properties,
                                          z => TupleStruct.Create(z.Name, z.PropertyType),
                                          z => TupleStruct.Create(z.Name, z.PropertyType),
                                          TupleStruct.Create)
                                      .ToList();

            pairs.Foreach(
                (TupleStruct<PropertyInfo, PropertyInfo> z) =>
                {
                    try
                    {
                        z.Item1.SetValue(arg1, z.Item2.GetValue(arg2, new object[0]), new object[0]);
                    }
                    catch (Exception e)
                    {
                        throw new System.ArgumentException($"Property:{z.Item1.Name}", e);
                    }
                });
        }

        [Obsolete("v10 This method will be protected in future")]
        public static Dictionary<string, object> ToPropertyDictionary(
            this object source,
            Func<string, string> propertyNameSelector = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var request = from property in source.GetType().GetProperties()
                          where !property.GetIndexParameters().Any()
                          let name = propertyNameSelector == null ? property.Name : propertyNameSelector(property.Name)
                          select name.ToKeyValuePair(property.GetValue(source, new object[0]));

            return request.ToDictionary();
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string Format(this string source, params object[] args)
        {
            return string.Format(source, args);
        }

        [Obsolete("v10 This method will be protected in future")]
        public static string TryModifiedString(this string source, Type valueType, CultureInfo cultureInfo)
        {
            if (null != source
                && (typeof(decimal) == valueType
                    || typeof(double) == valueType
                    || typeof(decimal?) == valueType
                    || typeof(double?) == valueType))
            {
                var currentDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;

                return source.Replace(",", currentDecimalSeparator).Replace(".", currentDecimalSeparator);
            }

            return source;
        }
    }
}
