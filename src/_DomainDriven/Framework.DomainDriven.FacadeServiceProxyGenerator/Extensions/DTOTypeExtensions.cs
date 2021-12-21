using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.OData;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    internal static class DTOTypeExtensions
    {
        public static IEnumerable<Type> GetFacadeMethodDTOTypes(this IEnumerable<Type> types)
        {
            var graph = new HashSet<Type>();

            types.FillFacadeMethodDTOTypes(graph);

            return graph;
        }

        private static void FillFacadeMethodDTOTypes(this IEnumerable<Type> types, HashSet<Type> graph)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            types.Foreach(type => FillFacadeMethodDTOTypes((Type)type, graph));
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
}
