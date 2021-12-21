using System.Collections.Generic;
using System.Linq;

namespace Framework.HierarchicalExpand
{
    public class PlainHierarchicalObjectExpander<TIdent> : IHierarchicalObjectExpander<TIdent>
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

        public Dictionary<TIdent, TIdent> ExpandWithParentsImplementation(IEnumerable<TIdent> idents, HierarchicalExpandType expandType)
        {
            return idents.ToDictionary(id => id, _ => default(TIdent));
        }
    }
}
