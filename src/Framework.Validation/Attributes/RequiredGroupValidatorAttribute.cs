using System;

namespace Framework.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredGroupValidatorAttribute : ClassValidatorAttribute
    {
        public readonly RequiredGroupValidatorMode Mode;


        public RequiredGroupValidatorAttribute(RequiredGroupValidatorMode mode)
        {
            this.Mode = mode;
        }


        public string GroupKey { get; set; }


        public override IClassValidator CreateValidator()
        {
            return new RequiredGroupValidator(this.Mode, this.GroupKey);
        }
    }
}