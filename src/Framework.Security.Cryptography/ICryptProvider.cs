using System.Runtime.InteropServices;

namespace Framework.Security.Cryptography
{
    //[ComVisible(true)]
    //[Guid("A72453A1-68E7-47B9-8601-28A5778B655F")]
    //[InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ICryptProvider
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}
