namespace Framework.BLL;

public interface IAuthorizationBLLContextContainer<out TAuthorizationBLLContext>
{
    TAuthorizationBLLContext Authorization { get; }
}
