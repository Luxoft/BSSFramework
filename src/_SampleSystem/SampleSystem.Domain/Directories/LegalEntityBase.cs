using Framework.Core;
using Framework.Relations;
using Framework.Restriction;

using SampleSystem.Domain.Ariba;

namespace SampleSystem.Domain.Directories;

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
        get => this.aribaStatus;
        protected internal set => this.aribaStatus = value;
    }

    public virtual TestObjForNested BaseObj
    {
        get => this.baseObj;
        set => this.baseObj = value;
    }

    public virtual IEnumerable<Address> Addresses => this.addresses;

    [Required]
    public virtual string NameEnglish
    {
        get => this.nameEnglish.TrimNull();
        set => this.nameEnglish = value.TrimNull();
    }

    ICollection<Address> IMaster<Address>.Details => (ICollection<Address>)this.Addresses;
}
