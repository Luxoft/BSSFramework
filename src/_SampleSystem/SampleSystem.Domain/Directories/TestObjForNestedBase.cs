using Framework.Core;

namespace SampleSystem.Domain.Directories;

public class TestObjForNestedBase : BaseDirectory
{
    private Period period;

    public virtual Period Period
    {
        get => this.period;
        set => this.period = value;
    }
}
