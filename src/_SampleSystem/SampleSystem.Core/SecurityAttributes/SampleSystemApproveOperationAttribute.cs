using Framework.Security;

namespace SampleSystem;

[AttributeUsage(AttributeTargets.Field)]
public class SampleSystemApproveOperationAttribute : ApproveOperationAttribute
{
    public SampleSystemApproveOperationAttribute(SampleSystemSecurityOperationCode operation)
            : base(operation)
    {
    }
}
