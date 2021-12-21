using System;

namespace Framework.Persistent
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MoneyAttribute : RoundDecimalAttribute
    {
        public MoneyAttribute() : base(NumberExtensions.MoneyRoundDecimals)
        {

        }
    }
}