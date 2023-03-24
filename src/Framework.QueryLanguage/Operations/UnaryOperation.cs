using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public enum UnaryOperation
{
    /// <summary>
    /// (! a)
    /// </summary>
    [EnumMember] Not,

    /// <summary>
    /// (+ a)
    /// </summary>
    [EnumMember] Plus,

    /// <summary>
    /// (- a)
    /// </summary>
    [EnumMember] Negate,
}
