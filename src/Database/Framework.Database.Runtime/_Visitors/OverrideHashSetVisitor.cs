using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.DictionaryCache;

namespace Framework.Database._Visitors;

public class OverrideHashSetVisitor<TIdent> : ExpressionVisitor
{
    private OverrideHashSetVisitor()
    {

    }

    public override Expression Visit(Expression node) => new InternalStateVisitor().Visit(node);

    public class InternalStateVisitor : ExpressionVisitor
    {
        private readonly IDictionaryCache<HashSet<TIdent>, Expression> constCache =

                new DictionaryCache<HashSet<TIdent>, Expression>(source => Expression.Constant(source.ToList()));


        public override Expression Visit(Expression? baseNode)
        {
            var request = from node in baseNode.ToMaybe()

                          from hashSet in node.GetConstantValue<HashSet<TIdent>>()

                          select this.constCache[hashSet];

            return request.GetValueOrDefault(() => base.Visit(baseNode));
        }
    }

    public static readonly OverrideHashSetVisitor<TIdent> Value = new OverrideHashSetVisitor<TIdent>();
}
