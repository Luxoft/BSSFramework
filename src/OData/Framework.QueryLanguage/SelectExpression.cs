namespace Framework.QueryLanguage;

public record SelectExpression(Expression Source, string PropertyName, string Alias) : PropertyExpression(Source, PropertyName)
{
    public override string ToString() => $"{this.Source}.[{this.PropertyName} {this.Alias}]";
}
