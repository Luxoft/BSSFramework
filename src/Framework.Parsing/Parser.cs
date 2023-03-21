namespace Framework.Parsing;

public delegate IResult<TInput, TValue> Parser<TInput, out TValue>(TInput input);
