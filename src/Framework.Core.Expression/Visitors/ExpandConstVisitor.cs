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
            var visitedBaseNode = base.Visit(baseNode);

            var request = from node in visitedBaseNode.ToMaybe()

                          from res in node.GetMemberConstExpression()

                          select res;


            return request.GetValueOrDefault(visitedBaseNode);
        }


        public static readonly ExpandConstVisitor Value = new ExpandConstVisitor();
    }
}