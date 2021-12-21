using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Configuration.Domain.Reports;
using Framework.Core;
using Framework.DomainDriven.SerializeMetadata;
using Framework.Exceptions;
using Framework.OData;
using Framework.Persistent;
using Framework.QueryLanguage;

using Expression = System.Linq.Expressions.Expression;

namespace Framework.CustomReports.Services
{
    public static class Extensions
    {
        public static PropertyMetadata GetProperty(this DomainTypeMetadata source, string propertyName)
        {
            return source.Properties.First(z => string.Equals(z.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));
        }

        internal static Expression Convert<TFrom, TResult>(this Expression<Func<TFrom, TResult>> source, Type nextFrom, params Tuple<string, string>[] propertyMappings)
        {
            var result = new ReplaceExpressionVisistor<TFrom, TResult>(nextFrom, propertyMappings).Visit(source);

            return result;
        }

        internal static MethodExpressionType ToMethodExpressionType(this string source)
        {
            switch (source.ToLower())
            {
                case "startswith":
                    return MethodExpressionType.StringStartsWith;

                case "contains":
                    return MethodExpressionType.StringContains;

                case "containsp":
                    return MethodExpressionType.PeriodContains;

                case "isintersected":
                    return MethodExpressionType.PeriodIsIntersected;

                case "isintersectedp":
                    return MethodExpressionType.PeriodIsIntersected;

                case "endswith":
                    return MethodExpressionType.StringEndsWith;

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Can't parse '{0}' to '{1}'", source, typeof(MethodExpressionType).Name));
            }
        }

        internal static QueryLanguage.ConstantExpression ToConstExpression(
          this string constValue,
          Type constType)
        {
            if (constType == null) throw new ArgumentNullException("constType");


            if (constValue == null)
            {
                return NullConstantExpression.Value;
            }

            if (constType == typeof(decimal))
            {
                return new DecimalConstantExpression(decimal.Parse(constValue));
            }


            if (constType == typeof(string))
            {
                return new StringConstantExpression((string)constValue);
            }

            if (constType == typeof(bool))
            {
                return new BooleanConstantExpression(bool.Parse(constValue));
            }

            if (constType == typeof(int))
            {
                return new Int32ConstantExpression(int.Parse(constValue));
            }

            if (constType == typeof(long))
            {
                return new Int64ConstantExpression(long.Parse(constValue));
            }

            if (constType == typeof(DateTime))
            {
                return new DateTimeConstantExpression(DateTime.Parse(constValue));
            }

            if (constType == typeof(Period))
            {
                var value = constValue.Contains("/")
                    ? constValue.Replace("/", "@")
                    : constValue;

                var period = Period.Parse(value);

                return new PeriodConstantExpression(period);
            }

            if (constType == typeof(Guid))
            {
                return new GuidConstantExpression(new Guid(constValue));
            }

            if (constType.IsEnum)
            {
                return new EnumConstantExpression((Enum)Enum.Parse(constType, constValue));
            }

            ////if (constType.IsNullable())
            ////{
            ////    return CreateNullableConstantMethod.MakeGenericMethod(constType.GetGenericArguments().Single())
            ////                                       .Invoke(null, new[] { constValue }) as ConstantExpression;

            ////}


            throw new NotImplementedException();
        }

        internal static string TryProcessNull(this string value, string filterOperator)
        {
            if (string.Equals(value, "null", StringComparison.InvariantCultureIgnoreCase) && string.Equals(filterOperator, "eqn", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return value;
        }

        public static MemberExpression ToPropertyExpr(this Expression source, string propertyPaths)
        {
            return source.ToPropertyExpr(propertyPaths.GetPropertyNameChain());
        }

        public static MemberExpression ToPropertyExpr(this Expression source, string[] propertyPaths)
        {
            var result = propertyPaths.Aggregate(source, Expression.Property);

            return (MemberExpression)result;
        }

        public static string GetVisualIdentityPropertyName(this ReportParameter source, ISystemMetadataTypeBuilder systemMetadataTypeBuilder)
        {
            var domainTypeHeader = new TypeHeader(source.TypeName);

            var domainTypeMetadata = systemMetadataTypeBuilder.SystemMetadata.GetDomainType(domainTypeHeader);

            var result = source.DisplayValueProperty.NullIfEmpty() ?? domainTypeMetadata.GetVisualPropertyMetadata()?.Name;

            return result;
        }

        internal static PropertyExpression ToPropertyExpr(this Framework.QueryLanguage.Expression source, string propertyPaths)
        {
            return source.ToPropertyExpr(propertyPaths.GetPropertyNameChain());
        }

        internal static PropertyExpression ToPropertyExpr(this Framework.QueryLanguage.Expression source, string[] propertyPaths)
        {
            var result = propertyPaths.Aggregate(source, (prev, path) => new PropertyExpression(prev, path));

            return (PropertyExpression)result;
        }

        internal static DomainProperty ToDomainProperty(this ReportParameter source, ISystemMetadataTypeBuilder systemMetadataTypeBuilder)
        {
            var domainTypeHeader = new TypeHeader(source.TypeName);

            var domainTypeMetadata = systemMetadataTypeBuilder.SystemMetadata.GetDomainType(domainTypeHeader);

            var visualIdentityProperty = source.GetVisualIdentityPropertyName(systemMetadataTypeBuilder);
            if (string.IsNullOrWhiteSpace(visualIdentityProperty))
            {
                throw new BusinessLogicException("Custom report parameter with name {0} has no DisplayValueProperty defined and underlying domain object not implemented IVisualIdentityObject or ICodeObject interface", source.Name);
            }

            var domainProperty = domainTypeMetadata.GetProperty(visualIdentityProperty);

            var domainType = systemMetadataTypeBuilder.TypeResolver.Resolve(domainTypeHeader, true);

            return new DomainProperty(domainType, domainProperty);
        }

        internal static DomainProperty ToVisualDomainProperty(this TypeHeader domainTypeHeader, ISystemMetadataTypeBuilder systemMetadataTypeBuilder)
        {
            var domainTypeMetadata = systemMetadataTypeBuilder.SystemMetadata.GetDomainType(domainTypeHeader);

            var domainProperty = domainTypeMetadata.GetVisualPropertyMetadata();

            var domainType = systemMetadataTypeBuilder.TypeResolver.Resolve(domainTypeHeader, true);

            return new DomainProperty(domainType, domainProperty);
        }

        public static PropertyMetadata GetVisualPropertyMetadata(this DomainTypeMetadata source)
        {
            return source.Properties.FirstOrDefault(z => z.IsVisualIdentity)
                                 ?? source.Properties.FirstOrDefault(z => string.Equals(z.Name, nameof(IVisualIdentityObject.Name), StringComparison.InvariantCultureIgnoreCase))
                                 ?? source.Properties.FirstOrDefault(z => string.Equals(z.Name, nameof(ICodeObject.Code), StringComparison.InvariantCultureIgnoreCase));

        }

        internal static OrderType ToOrderType(this int sortType)
        {
            if (sortType == 1)
            {
                return OrderType.Asc;
            }

            return OrderType.Desc;
        }

        public static IEnumerable<PropertyInfo> ToPropertyInfoChain(this Type domainType, string[] propertyPath)
        {
            var queue = new Queue<string>(propertyPath);
            var currentDomainType = domainType;

            IList<PropertyInfo> result = new List<PropertyInfo>();

            while (queue.Any())
            {
                var propertyName = queue.Dequeue();

                var property = currentDomainType.GetProperty(propertyName, StringComparison.CurrentCultureIgnoreCase, true);

                var expandPath = property.GetExpandPath(true);

                if (expandPath != null)
                {
                    currentDomainType = expandPath.Last().PropertyType;

                    result.AddRange(expandPath);
                }
                else
                {
                    result.AddRange(property);
                    currentDomainType = property.PropertyType;
                }
            }

            return result;
        }


        internal static Type TryToNullable(this Type source)
        {
            if (source.IsClass || source.IsNullable())
            {
                return source;
            }
            return typeof(Nullable<>).MakeGenericType(source);
        }

        public static MemberExpression TryIdentInject(this MemberExpression source)
        {
            //// TODO: extract id name from  IIdentityObject interface

            return typeof(IIdentityObject<Guid>).IsAssignableFrom(source.GetValueType()) ? Expression.Property(source, "Id") : source;
        }


        internal static string ToStandartPropertyName(this ReportProperty source)
        {

            return string.Join(string.Empty, source.GetPropertyNameChain());
        }

        internal static string[] GetPropertyNameChain(this ReportProperty source)
        {
            return source.PropertyPath.GetPropertyNameChain();
        }

        public static string[] GetPropertyNameChain(this string source)
        {
            return source.Split(new[] { '.', '/' });
        }


        internal static IEnumerable<PropertyMetadata> GetPropertyMetadataChain(
            this DomainTypeMetadata source,
            string propertyChain,
            SystemMetadata systemMetadata)
        {
            return source.GetPropertyMetadataChain(propertyChain.GetPropertyNameChain(), systemMetadata);
        }


        internal static IEnumerable<PropertyMetadata> GetPropertyMetadataChain(
            this DomainTypeMetadata source,
            string[] propertyChain,
            SystemMetadata systemMetadata)
        {
            var currentDomainMetadata = source;

            var propertyNames = new Queue<string>(propertyChain);

            while (propertyNames.Any())
            {
                var currentPropertyName = propertyNames.Dequeue();

                var currentProperty = currentDomainMetadata.Properties.FirstOrDefault(z => string.Equals(z.Name, currentPropertyName, StringComparison.InvariantCultureIgnoreCase));

                if (null == currentProperty)
                {
                    throw new ArgumentException($"Property:'{currentPropertyName}' not found in type:'{currentDomainMetadata}'");
                }


                yield return currentProperty;

                if (propertyNames.Any())
                {
                    currentDomainMetadata = systemMetadata.GetDomainType(currentProperty.TypeHeader);
                }
            }
        }
    }
}
