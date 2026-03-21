using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

public enum UnaryOperation
{
    /// <summary>
    /// (! a)
    /// </summary>
    Not,

    /// <summary>
    /// (+ a)
    /// </summary>
    Plus,

    /// <summary>
    /// (- a)
    /// </summary>
    Negate,
}
