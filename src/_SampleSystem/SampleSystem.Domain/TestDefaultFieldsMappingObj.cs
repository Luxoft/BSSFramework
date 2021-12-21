using System;
using System.ComponentModel;

namespace SampleSystem.Domain
{
    public class TestDefaultFieldsMappingObj : DomainObjectBase
    {
        public const string StringDefaultVal = "abcde";

        public const int IntDefaultVal = 123;

        public const DayOfWeek EnumDefaultVal = DayOfWeek.Thursday;

        [DefaultValue(StringDefaultVal)]
        public string StrVal { get; set; } = StringDefaultVal;

        [DefaultValue(IntDefaultVal)]
        public int IntVal { get; set; } = IntDefaultVal;

        [DefaultValue(EnumDefaultVal)]
        public DayOfWeek EnumVal { get; set; } = EnumDefaultVal;
    }
}
