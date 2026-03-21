namespace Framework.BLL.Context._Authorization;

public interface IAuthorizationBLLContextContainer<out TAuthorizationBLLContext>
{
    TAuthorizationBLLContext Authorization { get; }
}
