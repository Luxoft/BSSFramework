using Framework.DomainDriven.BLL;
using Framework.Security;

namespace SampleSystem.Domain;

[CustomContextSecurity]
[BLLViewRole]
public class TestCustomContextSecurityObj : BaseDirectory
{
}
