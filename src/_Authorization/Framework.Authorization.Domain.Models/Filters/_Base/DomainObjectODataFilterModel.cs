﻿using System.Linq.Expressions;

using Framework.DomainDriven;

namespace Framework.Authorization.Domain;

public abstract class DomainObjectODataFilterModel<TDomainObject> : DomainObjectBase, IDomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    public abstract Expression<Func<TDomainObject, bool>> ToFilterExpression();
}
