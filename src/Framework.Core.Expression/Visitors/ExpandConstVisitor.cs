using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core
{
    public class ExpandConstVisitor : ExpressionVisitor
    {
        private ExpandConstVisitor()
        {
        }

        public override Expression Visit(Expression baseNode)
        {
            var baseVisited = base.Visit(baseNode);

            var request = from node in baseVisited.ToMaybe()

                          from res in node.GetMemberConstExpression()

                          select res;

            return request
                   .GetValueOrDefault(baseVisited);
        }


        public static readonly ExpandConstVisitor Value = new ExpandConstVisitor();
    }
}
