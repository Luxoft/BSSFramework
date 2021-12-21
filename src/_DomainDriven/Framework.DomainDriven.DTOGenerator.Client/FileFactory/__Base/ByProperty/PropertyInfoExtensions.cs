using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    internal static class CustomPropertyExtensions
    {
        public static Func<CodeExpression, CodeExpression> GetOverrideValueExpression(this PropertyInfo property)
        {
            if (property.PropertyType == typeof(decimal))
            {
                var awayFromZeroRoundDecimalAttribute = property.GetCustomAttribute<AwayFromZeroRoundDecimalAttribute>();

                if (awayFromZeroRoundDecimalAttribute != null)
                {
                    var method = new Func<decimal, int, decimal>(NumberExtensions.AwayFromZeroRound).Method;

                    return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param, awayFromZeroRoundDecimalAttribute.Decimals.ToPrimitiveExpression());
                }

                if (property.HasAttribute<MoneyAttribute>())
                {
                    var method = new Func<decimal, decimal>(NumberExtensions.RoundMoney).Method;

                    return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param);
                }

                if (property.HasAttribute<CoeffAttribute>())
                {
                    var method = new Func<decimal, decimal>(NumberExtensions.RoundCoeff).Method;

                    return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param);
                }

                if (property.HasAttribute<PercentAttribute>())
                {
                    var method = new Func<decimal, decimal>(NumberExtensions.RoundPercent).Method;

                    return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param);
                }

                var roundAttribute = property.GetCustomAttribute<RoundDecimalAttribute>();

                if (roundAttribute != null)
                {
                    var method = new Func<decimal, decimal>(decimal.Round).Method;

                    return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param, new CodePrimitiveExpression(roundAttribute.Decimals));
                }
            }

            if (property.PropertyType == typeof(DateTime))
            {
                if (property.HasAttribute<DateAttribute>())
                {
                    return param => new CodePropertyReferenceExpression(param, "Date");
                }
            }

            if (property.HasAttribute<DirectoryPathAttribute>())
            {
                var method = new Func<string, string>(StringExtensions.ToDirectoryPath).Method;

                return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param);
            }

            if (property.PropertyType == typeof(string))
            {
                var method = new Func<string, string>(StringExtensions.TrimNull).Method;

                return param => new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(method.DeclaringType), method.Name, param);
            }

            return v => v;
        }
    }
}