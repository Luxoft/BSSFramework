using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Framework.Core;

namespace Framework.Configuration.Domain
{
    public abstract class DomainObjectMultiFilterModel<TDomainObject> : DomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        public override Expression<Func<TDomainObject, bool>> ToFilterExpression()
        {
            return this.ToFilterExpressionItems().BuildAnd();
        }

        protected abstract IEnumerable<Expression<Func<TDomainObject, bool>>> ToFilterExpressionItems();
    }
}