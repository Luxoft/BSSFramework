using System;
using Framework.Security.Cryptography;

namespace AttachmentsSampleSystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AttachmentsSampleSystemCryptAttribute : CryptAttribute
    {
        public AttachmentsSampleSystemCryptAttribute()
            :this(CryptSystem.AttachmentsSampleSystem)
        {
        }

        public AttachmentsSampleSystemCryptAttribute(CryptSystem system)
            : base(system)
        {

        }
    }
}