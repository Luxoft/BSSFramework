using System;
using Framework.Security.Cryptography;

namespace SampleSystem
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SampleSystemCryptAttribute : CryptAttribute
    {
        public SampleSystemCryptAttribute()
            :this(CryptSystem.SampleSystem)
        {
        }

        public SampleSystemCryptAttribute(CryptSystem system)
            : base(system)
        {

        }
    }
}