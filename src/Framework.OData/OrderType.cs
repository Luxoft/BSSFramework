using System.Runtime.Serialization;

namespace Framework.OData;

[DataContract]
public enum OrderType
{
    [EnumMember]
    Asc,

    [EnumMember]
    Desc
}
