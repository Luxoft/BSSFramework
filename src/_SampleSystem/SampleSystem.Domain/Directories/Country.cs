using Framework.BLL.Domain.Persistent.IdentityObject;
using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Restriction;

namespace SampleSystem.Domain.Directories;

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
        get => this.code.TrimNull();
        set => this.code = value.TrimNull();
    }

    [Required]
    public virtual string NameNative
    {
        get => this.nameNative.TrimNull();
        set => this.nameNative = value.TrimNull();
    }

    [Required]
    public virtual string Culture
    {
        get => this.culture.TrimNull();
        set => this.culture = value.TrimNull();
    }
}
