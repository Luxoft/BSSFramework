using System;
using System.Runtime.Serialization;

using Framework.Core;
using Framework.HierarchicalExpand;

namespace Framework.QueryLanguage;

[DataContract]
public class ExpandExpression : Expression
{
    public ExpandExpression(ConstantExpression filterElement, HierarchicalExpandType expandType, LambdaExpression filterLambda)
    {
        if (filterElement == null) throw new ArgumentNullException(nameof(filterElement));
        if (filterLambda == null) throw new ArgumentNullException(nameof(filterLambda));


        this.FilterElement = filterElement;
        this.ExpandType = expandType;
        this.FilterLambda = filterLambda;
    }


    [DataMember]
    public ConstantExpression FilterElement { get; private set; }

    [DataMember]
    public HierarchicalExpandType ExpandType { get; private set; }

    [DataMember]
    public LambdaExpression FilterLambda { get; private set; }


    public override int GetHashCode()
    {
        return base.GetHashCode() ^ this.ExpandType.GetHashCode() ^ this.FilterElement.GetHashCode();
    }

    protected override bool InternalEquals(Expression other)
    {
        return (other as ExpandExpression).Maybe(otherExpandExpression =>
                                                         this.ExpandType == otherExpandExpression.ExpandType
                                                         && this.FilterElement == otherExpandExpression.FilterElement
                                                         && this.FilterLambda == otherExpandExpression.FilterLambda);
    }
}
