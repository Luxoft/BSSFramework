namespace Framework.SecuritySystem;

public interface ISecurityOperationInfoSource
{
    SecurityOperationInfo GetSecurityOperationInfo(SecurityOperation securityOperation);
}
