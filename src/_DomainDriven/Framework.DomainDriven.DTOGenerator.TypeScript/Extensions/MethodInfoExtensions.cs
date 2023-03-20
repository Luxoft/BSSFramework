using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

public static class MethodInfoExtensions
{
    public static Type UnpackTaskType([NotNull] this Type type)
    {
        var taskRes = type.GetGenericTypeImplementationArgument(typeof(Task<>));

        if (taskRes != null)
        {
            return taskRes;
        }
        else if (type == typeof(Task))
        {
            return typeof(void);
        }
        else
        {
            return type;
        }
    }

    public static Type GetReturnTypeWithUnpackTask(this MethodInfo methodInfo)
    {
        return methodInfo.ReturnType.UnpackTaskType();
    }

    public static IEnumerable<(string Name, Type ParameterType)> GetParametersWithExpandAutoRequest([NotNull] this MethodInfo methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        return methodInfo.TryExtractAutoRequestParameter().Match(

                                                                 autoRequestParameter => from field in autoRequestParameter.ParameterType.GetFields()

                                                                     let propertyAttr = field.GetCustomAttribute<AutoRequestPropertyAttribute>()

                                                                     where propertyAttr != null

                                                                     orderby propertyAttr.OrderIndex

                                                                     select (field.Name, ParameterType:field.FieldType),

                                                                 () => methodInfo.GetParameters().Select(p => (p.Name, p.ParameterType)))

                         .Where(pair => pair.ParameterType != typeof(CancellationToken));
    }

    public static Maybe<ParameterInfo> TryExtractAutoRequestParameter([NotNull] this MethodInfo methodInfo)
    {
        if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

        return methodInfo.GetParameters().Where(pair => pair.ParameterType != typeof(CancellationToken))
                         .SingleMaybe()
                         .Where(p => p.ParameterType.HasAttribute<AutoRequestAttribute>());
    }
}
