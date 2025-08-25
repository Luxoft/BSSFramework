using System.Runtime.Serialization;

using CommonFramework;

using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class SelectExpression : PropertyExpression
{
    public SelectExpression(Expression source, string propertyName, string alias)
            : base(source, propertyName)
    {
        if (alias == null) throw new ArgumentNullException(nameof(alias));

        if (alias.TrimNull().IsEmpty())
        {
            throw new ArgumentOutOfRangeException(nameof(alias), "empty alias");
        }

        this.Alias = alias;
    }


    [DataMember]
    public string Alias { get; private set; }


    public override string ToString()
    {
        return $"{this.Source}.[{this.PropertyName} {this.Alias}]";
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.Alias.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return base.InternalEquals(other) && (other as SelectExpression).Maybe(otherSelectExpression =>

                                                                                       this.Alias == otherSelectExpression.Alias);
    }
}
