using System;
using System.Runtime.InteropServices;

namespace Framework.Security.Cryptography
{
    //[ComVisible(true)]
    //[Guid("C4E94923-D231-4EB5-8DD3-8A0E3679D9F9")]
    //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICryptService
    {
        ICryptProvider GetCryptProvider(string certName, string symmetricAlgorithmName);
    }

    public interface ICryptService<in TSystem>
    {
        ICryptProvider GetCryptProvider(TSystem system);
    }
}