using Framework.Core;
using Framework.Security;

namespace Framework.DomainDriven.BLL.Security;

public static class SecurityOperationCodeExtensions
{
    public static Maybe ToSecurityOperation(this Guid guid)
            where TSecurityOperationCode : struct, Enum
    {
        return Helper.ByGuidDictionary.GetMaybeValue(guid);
    }

    public static Guid ToGuid(this TSecurityOperationCode securityOperations)
            where TSecurityOperationCode : struct, Enum
    {
        return Helper.Dictionary[securityOperations].Guid;
    }

    public static string GetDescription(this TSecurityOperationCode securityOperations)
            where TSecurityOperationCode : struct, Enum
    {
        var securityOperationAttribute = Helper.Dictionary[securityOperations];

        if (string.IsNullOrWhiteSpace(securityOperationAttribute.Description))
        {
            return securityOperations.ToString();
        }

        return securityOperationAttribute.Description;
    }

    public static IReadOnlyDictionary<SecurityOperationAttribute> GetDictionary()
            where TSecurityOperationCode : struct, Enum
    {
        return Helper.Dictionary;
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

    private static Dictionary<Enum, SecurityOperationAttribute> GetUntypeDictionary()
            where TSecurityOperationCode : struct, Enum
    {
        return GetDictionary().ToDictionary(pair => (Enum)pair.Key, pair => pair.Value);
    }

    private static class Helper
            where TSecurityOperationCode : struct, Enum
    {
        public static readonly IReadOnlyDictionary<SecurityOperationAttribute> Dictionary = CreateDictionary();

        public static readonly IReadOnlyDictionary<Guid> ByGuidDictionary = CreateGuidDictionary();


        private static Dictionary<Guid> CreateGuidDictionary()
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

        private static Dictionary<SecurityOperationAttribute> CreateDictionary()
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
