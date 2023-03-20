using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Framework.Core.ExpressionComparers;

namespace Framework.Core;

internal class ListInitComparer : ExpressionComparer<ListInitExpression>
{
    private ListInitComparer()
    {

    }


    public override bool Equals(ListInitExpression x, ListInitExpression y)
    {
        return base.Equals(x, y)
               && ExpressionComparer.Value.Equals(x.NewExpression, y.NewExpression)
               && this.CompareElementInitList(x.Initializers, y.Initializers);
    }

    public override int GetHashCode(ListInitExpression obj)
    {
        return base.GetHashCode(obj);
    }

    private bool CompareElementInitList(ReadOnlyCollection<ElementInit> x, ReadOnlyCollection<ElementInit> y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.SequenceEqual(y, ElementInitComparer.Value);
    }


    public static readonly ListInitComparer Value = new ListInitComparer();
}
