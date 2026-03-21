namespace Framework.QueryLanguage;

public enum BinaryOperation
{
    Equal,

    GreaterThanOrEqual,

    LessThanOrEqual,

    NotEqual,

    GreaterThan,

    LessThan,

    Add,

    Subtract,

    Mul,

    Div,

    Mod,

    OrElse,

    AndAlso,

    ///// <summary>
    ///// Если добавлять в парсер, то надо будет учитывать ассоциативность
    ///// </summary>
    //Pow,
}
