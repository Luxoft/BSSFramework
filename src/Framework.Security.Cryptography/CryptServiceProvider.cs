using System;
using System.Text;

namespace Framework.Security.Cryptography
{
    public static class CryptServiceProvider
    {
        public static string Encrypt(this ICryptProvider cryptProvider, string text, Encoding encoding)
        {
            if (cryptProvider == null) throw new ArgumentNullException(nameof(cryptProvider));
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var textData = encoding.GetBytes(text);

            var encyptedData = cryptProvider.Encrypt(textData);

            return Convert.ToBase64String(encyptedData);
        }

        public static string Decrypt(this ICryptProvider cryptProvider, string encyptedText, Encoding encoding)
        {
            if (cryptProvider == null) throw new ArgumentNullException(nameof(cryptProvider));
            if (encyptedText == null) throw new ArgumentNullException(nameof(encyptedText));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var encyptedData = Convert.FromBase64String(encyptedText);

            var data = cryptProvider.Decrypt(encyptedData);

            return encoding.GetString(data);
        }
    }
}