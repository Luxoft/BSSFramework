using Framework.Core;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

[NotAuditedClass]
[IgnoreHbmMapping]
public class ControlSettingsParamValue : AuditPersistentDomainObjectBase, IDetail<ControlSettingsParam>
{
    private readonly ControlSettingsParam controlSettingsParam;

    private string value;
    private string valueTypeName;
    private string culture;


    protected ControlSettingsParamValue()
    {

    }

    public ControlSettingsParamValue(ControlSettingsParam parameter)
    {
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        this.controlSettingsParam = parameter;
        this.controlSettingsParam.AddDetail(this);
    }

    public virtual ControlSettingsParam ControlSettingsParam
    {
        get { return this.controlSettingsParam; }
    }

    [MaxLength]
    public virtual string Value
    {
        get { return this.value.TrimNull(); }
        set { this.value = value.TrimNull(); }
    }

    public virtual string ValueTypeName
    {
        get { return this.valueTypeName.TrimNull(); }
        set { this.valueTypeName = value.TrimNull(); }
    }

    public virtual string Culture
    {
        get { return this.culture.TrimNull(); }
        set { this.culture = value.TrimNull(); }
    }


    ControlSettingsParam IDetail<ControlSettingsParam>.Master
    {
        get { return this.controlSettingsParam; }
    }
}
