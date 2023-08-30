using System.Runtime.Serialization;
using Framework.QueryLanguage;

namespace Framework.OData;

[DataContract]
public class SelectOrder
{
    public SelectOrder(LambdaExpression path, OrderType orderType)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        this.Path = path;
        this.OrderType = orderType;
    }


    [DataMember]
    public OrderType OrderType { get; private set; }

    [DataMember]
    public LambdaExpression Path { get; private set; }
}
