using System;
using System.Runtime.InteropServices;

namespace Framework.Security.Cryptography
{
    public interface ICryptService
    {
        ICryptProvider GetCryptProvider(string certName, string symmetricAlgorithmName);
    }

    public interface ICryptService<in TSystem>
    {
        ICryptProvider GetCryptProvider(TSystem system);
    }
}
