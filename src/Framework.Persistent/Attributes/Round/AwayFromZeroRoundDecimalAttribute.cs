using System;

namespace Framework.Persistent
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AwayFromZeroRoundDecimalAttribute : NormalizeAttribute
    {
        public AwayFromZeroRoundDecimalAttribute(int decimals)
        {
            this.Decimals = decimals;
        }

        public int Decimals { get; private set; }
    }
}