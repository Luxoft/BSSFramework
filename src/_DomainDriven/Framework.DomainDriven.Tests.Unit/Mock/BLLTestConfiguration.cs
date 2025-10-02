using CommonFramework;

using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.DomainDriven.UnitTest.Mock;

public struct BLLTestConfiguration
{
    public static BLLTestConfiguration Create<TPersistentDomainObjectBase, TDomainObject, TBLL>(Lazy<TBLL> bllLazy)
            where TBLL : IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, Guid>
            where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        var safeBLLLazy = bllLazy.Maybe(z => new Lazy<object>(() => bllLazy.Value));

        return new BLLTestConfiguration(typeof(TDomainObject), safeBLLLazy);
    }

    public Type DomainObjectType { get; private set; }
    public Lazy<object> BllLazy { get; private set; }

    private BLLTestConfiguration(Type domainObjectType, Lazy<object> bllLazy)
            : this()
    {
        if (domainObjectType == null) throw new ArgumentNullException(nameof(domainObjectType));

        this.DomainObjectType = domainObjectType;
        this.BllLazy = bllLazy;
    }

}
