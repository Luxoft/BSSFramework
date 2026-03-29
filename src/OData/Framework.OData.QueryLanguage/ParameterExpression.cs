namespace Framework.OData.QueryLanguage;

public record ParameterExpression(string Name) : Expression
{
    public override string ToString() => this.Name;

    public static ParameterExpression Default { get; } = new("$arg");
}
