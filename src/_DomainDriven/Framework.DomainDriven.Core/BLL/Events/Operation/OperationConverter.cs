using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    internal static class OperationConverter<TOperation, TOtherOperation>

        where TOperation : struct, Enum
        where TOtherOperation : struct, Enum
    {
        public static readonly IReadOnlyDictionary<TOperation, TOtherOperation> Map = GetMap();


        private static Dictionary<TOperation, TOtherOperation> GetMap()
        {
            var request = from operation in EnumHelper.GetValues<TOperation>()

                          select operation.ToOperationMaybe<TOperation, TOtherOperation>()
                                          .Select(other => operation.ToKeyValuePair(other));


            return request.CollectMaybe().ToDictionary();
        }
    }
}