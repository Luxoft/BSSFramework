using System;

namespace Framework.Persistent;

[AttributeUsage(AttributeTargets.Property)]
public class PercentAttribute  : RoundDecimalAttribute
{
    public PercentAttribute() : base(NumberExtensions.PercentRoundDecimals)
    {

    }
}
