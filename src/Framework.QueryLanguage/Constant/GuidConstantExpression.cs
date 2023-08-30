using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public class GuidConstantExpression : ConstantExpression<System.Guid>
{
    public GuidConstantExpression(Guid value)
            : base(value)
    {

    }

    public override string ToString()
    {
        return $"'{this.Value}'";
    }
}
