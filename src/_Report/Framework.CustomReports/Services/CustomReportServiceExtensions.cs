using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Validation;

namespace Framework.CustomReports.Services
{
    public static class CustomReportServiceExtensions
    {
        public static ConstantExpression ToConstExprByType(this string value, MemberExpression memberExpression)
        {
            var targetType = memberExpression.GetValueType().GetNullableElementTypeOrSelf();

            object castedValue;

            if (targetType == typeof(Guid))
            {
                castedValue = new Guid(value);
            }
            else if (targetType == typeof(Period))
            {
                castedValue = Period.Parse(value);
            }
            else if (targetType.IsEnum)
            {
                castedValue = Enum.Parse(targetType, value);
            }
            else
            {
                try
                {
                    castedValue = TypeDescriptor.GetConverter(targetType).ConvertFromString(value);
                }
                catch (Exception e)
                {
                    throw new ValidationException($"Can't parse '{value}' to type '{targetType}'", e);
                }
            }

            return Expression.Constant(castedValue, memberExpression.GetValueType());
        }

        public static Type GetValueType(this MemberExpression source)
        {
            return ((PropertyInfo)source.Member).PropertyType;
        }
    }
}
