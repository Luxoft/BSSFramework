using System;

namespace Framework.Security.Cryptography
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class CryptAttribute : Attribute
    {
        protected CryptAttribute(Enum system)
        {
            this.System = system;
        }


        public Enum System { get; private set; }
    }
}