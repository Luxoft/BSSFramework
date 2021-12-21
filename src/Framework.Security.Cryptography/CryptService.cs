using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Framework.Core;

namespace Framework.Security.Cryptography
{
    //[ComVisible(true)]
    //[Guid("9C092764-AA04-4936-AD15-A7F7CC679F2A")]
    //[ClassInterface(ClassInterfaceType.AutoDispatch)]
    //[ComSourceInterfaces(typeof(ICryptService))]
    //[ProgId(@"AddIn.CryptService")]
    public class CryptService : ICryptService
    {
        private readonly IDictionaryCache<string, KeyValuePair<byte[], byte[]>> _certHashCache;


        public CryptService()
        {
            this._certHashCache = new DictionaryCache<string, KeyValuePair<byte[], byte[]>>(certName =>
            {
                var algorithmKeys = certName.GetAlgorithmKeys(Encoding.UTF8);

                using (var hashAlg = HashAlgorithm.Create(this.GetHashAlgorithmName()))
                {
                    return new KeyValuePair<byte[], byte[]>(hashAlg.ComputeHash(algorithmKeys.Key), hashAlg.ComputeHash(algorithmKeys.Value));
                }
            }).WithLock();
        }


        protected virtual string GetHashAlgorithmName()
        {
            return "MD5";
        }


        public ICryptProvider GetCryptProvider(string certName, string symmetricAlgorithmName)
        {
            if (certName == null) throw new ArgumentNullException(nameof(certName));
            if (symmetricAlgorithmName == null) throw new ArgumentNullException(nameof(symmetricAlgorithmName));

            var pair = this._certHashCache[certName];

            return new CryptProvider(() => SymmetricAlgorithm.Create(symmetricAlgorithmName), pair.Value, pair.Key);
        }
    }

    public class CryptService<TSystem> : ICryptService<TSystem>
    {
        private readonly CryptService _innerService = new CryptService();


        public CryptService()
        {

        }


        protected virtual string GetCertName(TSystem system)
        {
            return system.ToString();
        }

        protected virtual string GetSymmetricAlgorithmName(TSystem system)
        {
            return "TripleDES";
        }


        public ICryptProvider GetCryptProvider(TSystem system)
        {
            return this._innerService.GetCryptProvider(this.GetCertName(system), this.GetSymmetricAlgorithmName(system));
        }
    }
}