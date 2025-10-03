using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public enum MethodExpressionType
{
    [EnumMember]
    StringContains,

    [EnumMember]
    StringStartsWith,

    [EnumMember]
    StringEndsWith,

    [EnumMember]
    CollectionAny,

    [EnumMember]
    CollectionAll,
}
