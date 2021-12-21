namespace Framework.DomainDriven.BLL.Security
{
    public interface IAuthorizationBLLContextContainer<out TAuthorizationBLLContext>
    {
        TAuthorizationBLLContext Authorization { get; }
    }
}