using System;
using System.Runtime.Serialization;
using Framework.Core;

namespace Framework.QueryLanguage;

[DataContract]
public class ExpandContainsExpression : Expression
{
    public ExpandContainsExpression(ConstantExpression expandType, ConstantExpression filterElement, Expression source)
    {
        if (filterElement == null) throw new ArgumentNullException(nameof(filterElement));
        if (source == null) throw new ArgumentNullException(nameof(source));


        this.ExpandType = expandType;
        this.FilterElement = filterElement;
        this.Source = source;
    }


    [DataMember]
    public ConstantExpression ExpandType { get; private set; }

    [DataMember]
    public ConstantExpression FilterElement { get; private set; }

    [DataMember]
    public Expression Source { get; private set; }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.ExpandType.GetHashCode() ^ this.FilterElement.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as ExpandContainsExpression).Maybe(otherExpandExpression =>
                                                                 this.ExpandType == otherExpandExpression.ExpandType
                                                                 && this.FilterElement == otherExpandExpression.FilterElement
                                                                 && this.Source == otherExpandExpression.Source);
    }
}
