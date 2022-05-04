using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.HierarchicalExpand;

public interface IHierarchicalObjectQueryableExpander<TIdent>
{
    IEnumerable<TIdent> ExpandEnumerable(IEnumerable<TIdent> baseIdents, HierarchicalExpandType expandType);

    IQueryable<TIdent> ExpandQueryable(IQueryable<TIdent> idents, HierarchicalExpandType expandType);

    Expression<Func<IEnumerable<TIdent>, IEnumerable<TIdent>>> GetExpandExpression(HierarchicalExpandType expandType);
}
