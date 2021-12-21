using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<(string Name, Type ParameterType)> GetParametersWithExpandAutoRequest([NotNull] this MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.TryExtractAutoRequestParameter().Match(

                autoRequestParameter => from field in autoRequestParameter.ParameterType.GetFields()

                                        let propertyAttr = field.GetCustomAttribute<AutoRequestPropertyAttribute>()

                                        where propertyAttr != null

                                        orderby propertyAttr.OrderIndex

                                        select (field.Name, field.FieldType),

                () => methodInfo.GetParameters().Select(p => (p.Name, p.ParameterType)));
        }

        public static Maybe<ParameterInfo> TryExtractAutoRequestParameter([NotNull] this MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.GetParameters()
                             .SingleMaybe()
                             .Where(p => p.ParameterType.HasAttribute<AutoRequestAttribute>());
        }
    }
}
