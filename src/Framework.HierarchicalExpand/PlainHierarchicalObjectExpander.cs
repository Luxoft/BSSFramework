using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.HierarchicalExpand
{
    public class PlainHierarchicalObjectExpander<TIdent> : IHierarchicalObjectExpander<TIdent>, IHierarchicalObjectQueryableExpander<TIdent>
    {
        public IEnumerable<TIdent> Expand(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
        {
            return idents;
        }

        public Dictionary<TIdent, TIdent> ExpandWithParents(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
        {
            return this.ExpandWithParentsImplementation(idents, expandType);
        }

        public Dictionary<TIdent, TIdent> ExpandWithParents(IQueryable<TIdent> idents, HierarchicalExpandType expandType)
        {
            return this.ExpandWithParentsImplementation(idents, expandType);
        }

        public Dictionary<TIdent, TIdent> ExpandWithParentsImplementation(IEnumerable<TIdent> idents, HierarchicalExpandType _)
        {
            return idents.ToDictionary(id => id, _ => default(TIdent));
        }

        public Expression<Func<IEnumerable<TIdent>, IEnumerable<TIdent>>> GetExpandExpression(HierarchicalExpandType expandType)
        {
            return v => v;
        }
    }
}
