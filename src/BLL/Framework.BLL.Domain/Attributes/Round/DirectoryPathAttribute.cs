using Framework.BLL.Domain.Attributes.Round.Base;

namespace Framework.BLL.Domain.Attributes.Round;

[AttributeUsage(AttributeTargets.Property)]
public class DirectoryPathAttribute : NormalizeAttribute
{
    public DirectoryPathAttribute()
    {

    }
}
