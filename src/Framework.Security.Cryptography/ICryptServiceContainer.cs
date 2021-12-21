namespace Framework.Security.Cryptography
{
    public interface ICryptServiceContainer<in TSystem>
    {
        ICryptService<TSystem> CryptService { get; }
    }
}