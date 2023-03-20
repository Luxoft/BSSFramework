using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.OData;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

public static class TypeExtensions
{
    public static IEnumerable<MethodInfo> ExtractContractMethods([NotNull] this Type contractType)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));

        return contractType.IsInterface
                       ? contractType.GetAllInterfaceMethods()
                       : contractType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                     .Where(
                                            m => !m.HasAttribute<NonActionAttribute>()
                                                 && !m.IsSpecialName
                                                 && m.DeclaringType != typeof(object));
    }

    internal static IEnumerable<Type> GetFacadeMethodDTOTypes(this IEnumerable<Type> types)
    {
        var graph = new HashSet<Type>();

        types.FillFacadeMethodDTOTypes(graph);

        return graph;
    }

    private static void FillFacadeMethodDTOTypes(this IEnumerable<Type> types, HashSet<Type> graph)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));

        types.Foreach(type => FillFacadeMethodDTOTypes(type, graph));
    }

    private static void FillFacadeMethodDTOTypes(this Type type, HashSet<Type> graph)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (graph == null) throw new ArgumentNullException(nameof(graph));

        var genericElementType = type.GetCollectionOrArrayElementType()
                                 ?? type.GetNullableElementType()
                                 ?? type.GetMaybeElementType()
                                 ?? type.GetGenericTypeImplementationArgument(typeof(SelectOperationResult<>));

        if (genericElementType != null)
        {
            genericElementType.FillFacadeMethodDTOTypes(graph);
        }
        else if (graph.Add(type))
        {
            if (type.IsEnum)
            {
                Enum.GetUnderlyingType(type).FillFacadeMethodDTOTypes(graph);
            }

            //else
            //{
            //    type.GetProperties().Select(property => property.PropertyType).FillFacadeMethodDTOTypes(graph);
            //}
        }
    }
}
