using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Tracking.LegacyValidators;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

[NotAuditedClass]
public class ControlSettings : BaseDirectory,
                               IMaster<ControlSettings>,
                               IMaster<ControlSettingsParam>,
                               IDetail<ControlSettings>
{
    private readonly ICollection<ControlSettings> children = new List<ControlSettings>();

    private readonly ICollection<ControlSettingsParam> controlSettingsParams = new List<ControlSettingsParam>();

    private ControlSettings? parent; // readonly

    private readonly string accountName;

    private ControlSettingsType type;

    protected ControlSettings()
    {
    }

    /// <summary>
    /// not used cto'r
    /// </summary>
    /// <param name="parent"></param>
    public ControlSettings(ControlSettings? parent)
            : this(parent, null)
    {

    }

    public ControlSettings(ControlSettings? parent, string? accountName)
    {
        if (accountName == null) throw new ArgumentNullException(nameof(accountName));

        if (parent != null)
        {
            this.parent.AddDetail(this);
        }

        this.accountName = accountName.TrimNull();
    }

    [AutoMapping(false)]
    public virtual IEnumerable<ControlSettings> Children
    {
        get { return this.children; }
    }


    public virtual IEnumerable<ControlSettingsParam> ControlSettingsParams
    {
        get { return this.controlSettingsParams; }
    }

    /// <summary>
    /// Supposed to be set from dto only.
    /// </summary>
    [FixedPropertyValidator]
    public virtual ControlSettings? Parent
    {
        get { return this.parent; }
        set { this.parent = value; }
    }

    public virtual ControlSettingsType Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    [MaxLength]
    public override string Name
    {
        get { return base.Name; }
        set { base.Name = value; }
    }

    public virtual string AccountName
    {
        get { return this.accountName.TrimNull(); }
    }

    ICollection<ControlSettings> IMaster<ControlSettings>.Details
    {
        get { return this.children; }
    }

    ICollection<ControlSettingsParam> IMaster<ControlSettingsParam>.Details
    {
        get { return this.controlSettingsParams; }
    }

    ControlSettings IDetail<ControlSettings>.Master
    {
        get { return this.parent; }
    }
}
