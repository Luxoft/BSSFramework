using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Security;

namespace Framework.DomainDriven.BLL.Security
{
    public static class SecurityOperationCodeExtensions
    {
        public static Maybe<TSecurityOperationCode> ToSecurityOperation<TSecurityOperationCode>(this Guid guid)
            where TSecurityOperationCode : struct, Enum
        {
            return Helper<TSecurityOperationCode>.ByGuidDictionary.GetMaybeValue(guid);
        }

        public static Guid ToGuid<TSecurityOperationCode>(this TSecurityOperationCode securityOperations)
            where TSecurityOperationCode : struct, Enum
        {
            return Helper<TSecurityOperationCode>.Dictionary[securityOperations].Guid;
        }

        public static string GetDescription<TSecurityOperationCode>(this TSecurityOperationCode securityOperations)
            where TSecurityOperationCode : struct, Enum
        {
            var securityOperationAttribute = Helper<TSecurityOperationCode>.Dictionary[securityOperations];

            if (string.IsNullOrWhiteSpace(securityOperationAttribute.Description))
            {
                return securityOperations.ToString();
            }

            return securityOperationAttribute.Description;
        }

        public static IReadOnlyDictionary<TSecurityOperationCode, SecurityOperationAttribute> GetDictionary<TSecurityOperationCode>()
            where TSecurityOperationCode : struct, Enum
        {
            return Helper<TSecurityOperationCode>.Dictionary;
        }

        public static IReadOnlyDictionary<Enum, SecurityOperationAttribute> GetDictionary(Type type, bool withBase, bool removeDuplicate)
        {
            var getUntypeDictionaryMethod = new Func<Dictionary<Enum, SecurityOperationAttribute>>(GetUntypeDictionary<SecurityOperationCode>)
                                           .Method
                                           .GetGenericMethodDefinition();

            var types = withBase ? type.GetSecurityOperationTypes() : new[] { type };

            var dict = types.Aggregate(new Dictionary<Enum, SecurityOperationAttribute>(), (d, t) =>
            {
                var operations = getUntypeDictionaryMethod.MakeGenericMethod(t).Invoke<Dictionary<Enum, SecurityOperationAttribute>>(null);

                return d.Union(operations).ToDictionary();
            });

            if (removeDuplicate)
            {
                return dict.Distinct(pair => pair.Value.Guid).ToDictionary();
            }
            else
            {
                return dict;
            }
        }

        private static Dictionary<Enum, SecurityOperationAttribute> GetUntypeDictionary<TSecurityOperationCode>()
            where TSecurityOperationCode : struct, Enum
        {
            return GetDictionary<TSecurityOperationCode>().ToDictionary(pair => (Enum)pair.Key, pair => pair.Value);
        }

        private static class Helper<TSecurityOperationCode>
            where TSecurityOperationCode : struct, Enum
        {
            public static readonly IReadOnlyDictionary<TSecurityOperationCode, SecurityOperationAttribute> Dictionary = CreateDictionary();

            public static readonly IReadOnlyDictionary<Guid, TSecurityOperationCode> ByGuidDictionary = CreateGuidDictionary();


            private static Dictionary<Guid, TSecurityOperationCode> CreateGuidDictionary()
            {
                var groupedRequest = from pair in Dictionary

                                     group pair.Key by pair.Value.Guid;


                var pairs = groupedRequest.Partial(g => g.Count() > 1, (duplicateOperations, uniOperations) => new { DuplicateOperations = duplicateOperations, UniOperations = uniOperations });

                if (pairs.DuplicateOperations.Any())
                {
                    throw new Exception($"Duplicate operation guids: {pairs.DuplicateOperations.Join(" | ", g => $"[{g.Key}: {g.Join(", ")}]")}");
                }

                return pairs.UniOperations.ToDictionary(g => g.Key, g => g.Single());
            }

            private static Dictionary<TSecurityOperationCode, SecurityOperationAttribute> CreateDictionary()
            {
                var requst = from fieldName in Enum.GetNames(typeof(TSecurityOperationCode))

                             let field = typeof(TSecurityOperationCode).GetField(fieldName)

                             let guidAttribute = field.GetCustomAttribute<SecurityOperationAttribute>()

                             where guidAttribute != null

                             let fieldValue = (TSecurityOperationCode)Enum.Parse(typeof(TSecurityOperationCode), field.Name)

                             select fieldValue.ToKeyValuePair(guidAttribute);


                return requst.ToDictionary();
            }
        }
    }
}
