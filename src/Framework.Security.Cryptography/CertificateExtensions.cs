using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Framework.Core;

namespace Framework.Security.Cryptography
{
    internal static class CertificateExtensions
    {
        public static KeyValuePair<byte[], byte[]> GetAlgorithmKeys(this string certName, Encoding encoding)
        {
            if (certName == null) throw new ArgumentNullException(nameof(certName));

            var store = new X509Store(StoreLocation.LocalMachine);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                var cert = store.Certificates.Find(X509FindType.FindBySubjectName, certName, false)
                    .Cast<X509Certificate2>()
                    .Single(() => new Exception($"Cereficate with name \"{certName}\" not found in storage \"{store.Name}\""),
                            () => new Exception($"To many cereficates with name \"{certName}\" in storage \"{store.Name}\""));

                var publicXml = cert.PrivateKey.ToXmlString(false);

                var privateXml = cert.PrivateKey.ToXmlString(true);

                return new KeyValuePair<byte[], byte[]>(encoding.GetBytes(publicXml), encoding.GetBytes(privateXml));
            }
            finally
            {
                store.Close();
            }
        }
    }
}

