namespace Framework.OData.QueryLanguage;

public record PropertyExpression(Expression Source, string PropertyName) : Expression
{
    public override string ToString() => $"{this.Source}.{this.PropertyName}";
}
