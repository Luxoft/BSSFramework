using System;
using System.Linq;

namespace Framework.QueryableSource;

public interface IQueryableSource<in TPersistentDomainObjectBase>
{
    IQueryable<TDomainObject> GetQueryable<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;
}
