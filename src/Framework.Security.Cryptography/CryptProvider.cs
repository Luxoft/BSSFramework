using System;
using System.IO;
using System.Security.Cryptography;
using Framework.Core;

namespace Framework.Security.Cryptography
{
    public class CryptProvider : ICryptProvider
    {
        private readonly Func<SymmetricAlgorithm> _getSymmetricAlgorithm;
        private readonly byte[] _key;
        private readonly byte[] _vector;

        public CryptProvider (Func<SymmetricAlgorithm> getSymmetricAlgorithm, byte[] key, byte[] vector)
        {
            if (getSymmetricAlgorithm == null) throw new ArgumentNullException(nameof(getSymmetricAlgorithm));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            this._getSymmetricAlgorithm = getSymmetricAlgorithm;
            this._key = key;
            this._vector = vector;

            //var base64Key = Convert.ToBase64String(this._key);
            //var base64Vector = Convert.ToBase64String(this._vector);
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));


            using (var symmetricAlgorithm = this._getSymmetricAlgorithm())
            using (var cryptor = symmetricAlgorithm.CreateEncryptor(this._key, this._vector))
            using (var memStream = new MemoryStream())
            using (var criptoStream = new CryptoStream(memStream, cryptor, CryptoStreamMode.Write))
            {
                criptoStream.Write(data, 0, data.Length);
                criptoStream.FlushFinalBlock();

                return memStream.ToArray();
            }
        }


        public byte[] Decrypt(byte[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));


            using (var symmetricAlgorithm = this._getSymmetricAlgorithm())
            using (var cryptor = symmetricAlgorithm.CreateDecryptor(this._key, this._vector))
            using (var memStream = new MemoryStream(data))
            using (var criptoStream = new CryptoStream(memStream, cryptor, CryptoStreamMode.Read))
            {
                return criptoStream.ToArray(data.Length);
            }
        }
    }
}
