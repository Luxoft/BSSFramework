using System;

namespace Framework.Persistent
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CoeffAttribute : RoundDecimalAttribute
    {
        public CoeffAttribute() : base(NumberExtensions.CoeffRoundDecimals)
        {

        }
    }
}