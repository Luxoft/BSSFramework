using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven.BLL;

public class OverrideHashSetVisitor<TIdent> : ExpressionVisitor
{
    private OverrideHashSetVisitor()
    {

    }

    public override Expression Visit(Expression node)
    {
        return new InternalStateVisitor().Visit(node);
    }

    public class InternalStateVisitor : ExpressionVisitor
    {
        private readonly IDictionaryCache<HashSet<TIdent>, Expression> _constCache =

                new DictionaryCache<HashSet<TIdent>, Expression>(source => Expression.Constant(source.ToList()));


        public override Expression Visit(Expression baseNode)
        {
            var request = from node in baseNode.ToMaybe()

                          from hashSet in node.GetDeepMemberConstValue<HashSet<TIdent>>()

                          select this._constCache[hashSet];

            return request.GetValueOrDefault(() => base.Visit(baseNode));
        }
    }

    public static readonly OverrideHashSetVisitor<TIdent> Value = new OverrideHashSetVisitor<TIdent>();
}
