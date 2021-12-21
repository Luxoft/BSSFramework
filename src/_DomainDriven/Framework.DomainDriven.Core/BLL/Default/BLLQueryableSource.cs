using System.Linq;

using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.DomainDriven.BLL
{
    public class BLLQueryableSource<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TIdent> : BLLContextContainer<TBLLContext>, IQueryableSource<TPersistentDomainObjectBase>

        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
    {
        public BLLQueryableSource(TBLLContext context)
            : base(context)
        {

        }

        public IQueryable<TDomainObject> GetQueryable<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return this.Context.Logics.Default.Create<TDomainObject>().GetUnsecureQueryable();
        }
    }
}
