using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole, BLLSaveRole, BLLRemoveRole]
[UniqueGroup]
public class Country : BaseDirectory, ICodeObject
{
    private string code;
    private string nameNative;
    private string culture;

    [Required]
    public virtual string Code
    {
        get
        {
            return this.code.TrimNull();
        }

        set
        {
            this.code = value.TrimNull();
        }
    }

    [Required]
    public virtual string NameNative
    {
        get
        {
            return this.nameNative.TrimNull();
        }

        set
        {
            this.nameNative = value.TrimNull();
        }
    }

    [Required]
    public virtual string Culture
    {
        get
        {
            return this.culture.TrimNull();
        }

        set
        {
            this.culture = value.TrimNull();
        }
    }
}
