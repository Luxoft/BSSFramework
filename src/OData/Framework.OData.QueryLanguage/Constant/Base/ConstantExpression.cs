namespace Framework.OData.QueryLanguage.Constant.Base;

public abstract record ConstantExpression : Expression
{
    public abstract object? GetRawValue();

    public override string? ToString() => this.GetRawValue()?.ToString();
}

public abstract record ConstantExpression<TValue>(TValue Value) : ConstantExpression
{
    public override object? GetRawValue() => this.Value;
}
