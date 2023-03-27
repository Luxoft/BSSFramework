using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public enum BinaryOperation
{
    [EnumMember] Equal,

    [EnumMember] GreaterThanOrEqual,

    [EnumMember] LessThanOrEqual,

    [EnumMember] NotEqual,

    [EnumMember] GreaterThan,

    [EnumMember] LessThan,

    [EnumMember] Add,

    [EnumMember] Subtract,

    [EnumMember] Mul,

    [EnumMember] Div,

    [EnumMember] Mod,

    [EnumMember] OrElse,

    [EnumMember] AndAlso,

    ///// <summary>
    ///// Если добавлять в парсер, то надо будет учитывать ассоциативность
    ///// </summary>
    //[EnumMember] Pow,
}
