using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
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
        PeriodContains,

        [EnumMember]
        PeriodIsIntersected,

        [EnumMember]
        CollectionAny,

        [EnumMember]
        CollectionAll,

        [EnumMember]
        CalendarPeriodContains,

        [EnumMember]
        CalendarPeriodIsIntersected
    }
}