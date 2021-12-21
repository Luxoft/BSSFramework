using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Core;
using Framework.CustomReports.Attributes;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.CustomReports.Domain
{
    public static class CustomReportExtensions
    {
        private class CustomReportParameter : ICustomReportParameter
        {
            public CustomReportParameter(string displayValueProperty, string name, bool isRequired, int order, string typeName, bool isCollection)
            {
                this.DisplayValueProperty = displayValueProperty;
                this.Name = name;
                this.IsRequired = isRequired;
                this.Order = order;
                this.TypeName = typeName;
                this.IsCollection = isCollection;
            }

            public string Name { get; }

            public string TypeName { get; }

            public bool IsRequired { get; }

            public int Order { get; }

            public string DisplayValueProperty { get; }

            public bool IsCollection { get; }
        }
        public static IEnumerable<ICustomReportParameter> GetReportParameters<TSecurityOperationCode>(this ICustomReport<TSecurityOperationCode> source)
        {
            var parameterType = source.ParameterType;

            return parameterType
                .GetProperties()
                .Select(z => new CustomReportParameter(

                    z.GetParameterVisualIdentity(),

                    CustomAttributeProviderExtensions.GetCustomAttributes<ReportParameterDisplayNameAttribute>(z).FirstOrDefault().Maybe(q => q.Value, z.Name),

                    z.HasAttribute<RequiredAttribute>(),

                    CustomAttributeProviderExtensions.GetCustomAttributes<ReportParameterOrderAttribute>(z).FirstOrDefault().Maybe(q => q.Value, 0),

                    z.PropertyType.GetCollectionOrArrayElementType()?.Name ?? z.PropertyType.Name,

                    z.PropertyType.IsCollectionOrArray()));
        }

        private static string GetParameterVisualIdentity(this PropertyInfo source)
        {
            var result = source
                .GetCustomAttributes<ReportParameterVisualIdentityAttribute>()
                .FirstOrDefault()
                .Maybe(q => q.Value, string.Empty);

            if (string.IsNullOrWhiteSpace(result))
            {
                var expandedPropertyType = source.PropertyType.GetCollectionOrArrayElementType() ?? source.PropertyType;
                result = expandedPropertyType.GetProperties().FirstOrDefault(q => q.GetCustomAttributes<VisualIdentityAttribute>().Any()).Maybe(q => q.Name);
            }

            return result;
        }
    }
}
