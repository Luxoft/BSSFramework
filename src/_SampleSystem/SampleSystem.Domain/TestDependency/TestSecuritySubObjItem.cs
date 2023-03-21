using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[DependencySecurity(typeof(TestSecurityObjItem), nameof(InnerMaster))]
public class TestSecuritySubObjItem : BaseDirectory, IDetail<TestSecurityObjItem>
{
    private TestSecurityObjItem innerMaster;

    protected TestSecuritySubObjItem()
    {

    }

    public TestSecuritySubObjItem(TestSecurityObjItem master)
    {
        if (master == null) throw new ArgumentNullException(nameof(master));

        this.innerMaster = master;
        this.innerMaster.AddDetail(this);
    }


    public virtual TestSecurityObjItem InnerMaster => this.innerMaster;

    TestSecurityObjItem IDetail<TestSecurityObjItem>.Master => this.InnerMaster;
}
