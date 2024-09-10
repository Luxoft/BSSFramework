using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

public class LegalEntityBase : BaseDirectory, IMaster<Address>
{
    private readonly ICollection<Address> addresses = new List<Address>();

    private string nameEnglish;

    private TestObjForNested baseObj;

    private RevenueDocumentAribaStatus aribaStatus = RevenueDocumentAribaStatus.CreateDefault();

    public LegalEntityBase()
    {
    }

    public virtual RevenueDocumentAribaStatus AribaStatus
    {
        get { return this.aribaStatus; }
        protected internal set { this.aribaStatus = value; }
    }

    public virtual TestObjForNested BaseObj
    {
        get { return this.baseObj; }
        set { this.baseObj = value; }
    }

    public virtual IEnumerable<Address> Addresses
    {
        get { return this.addresses; }
    }

    [Required]
    public virtual string NameEnglish
    {
        get { return this.nameEnglish.TrimNull(); }
        set { this.nameEnglish = value.TrimNull(); }
    }

    ICollection<Address> IMaster<Address>.Details
    {
        get { return (ICollection<Address>)this.Addresses; }
    }
}
