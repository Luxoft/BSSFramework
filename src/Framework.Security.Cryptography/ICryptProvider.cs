using System.Runtime.InteropServices;

namespace Framework.Security.Cryptography
{
    public interface ICryptProvider
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}
