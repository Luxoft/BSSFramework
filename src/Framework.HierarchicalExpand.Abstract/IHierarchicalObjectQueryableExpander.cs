using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.HierarchicalExpand;

public interface IHierarchicalObjectQueryableExpander<TIdent>
{
    Expression<Func<IEnumerable<TIdent>, IEnumerable<TIdent>>> GetExpandExpression(HierarchicalExpandType expandType);
}
