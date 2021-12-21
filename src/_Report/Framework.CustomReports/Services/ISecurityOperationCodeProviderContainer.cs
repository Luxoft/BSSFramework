namespace Framework.CustomReports.Domain
{
    public interface ISecurityOperationCodeProviderContainer<TSecurityOperationCode>
    {
        ISecurityOperationCodeProvider<TSecurityOperationCode> SecurityOperationCodeProvider { get; }
    }
}