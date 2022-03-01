using Framework.DomainDriven.BLL;
using Framework.Security;

namespace WorkflowSampleSystem.Domain
{
    [CustomContextSecurity]
    [BLLViewRole]
    public class TestCustomContextSecurityObj : BaseDirectory
    {
    }
}
