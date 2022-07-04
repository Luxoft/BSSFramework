using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Framework.HierarchicalExpand;

public interface IHierarchicalObjectQueryableExpander<TIdent>
{
    Expression<Func<IEnumerable<TIdent>, IEnumerable<TIdent>>> GetExpandExpression(HierarchicalExpandType expandType);

    Expression<Func<TIdent, IEnumerable<TIdent>>> TryGetSingleExpandExpression(HierarchicalExpandType expandType);
}
