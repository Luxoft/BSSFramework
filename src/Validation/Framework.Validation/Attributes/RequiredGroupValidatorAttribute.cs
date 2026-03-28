namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiredGroupValidatorAttribute(RequiredGroupValidatorMode mode) : ClassValidatorAttribute
{
    public readonly RequiredGroupValidatorMode Mode = mode;

    public string GroupKey { get; set; }


    public override IClassValidator CreateValidator() => new RequiredGroupValidator(this.Mode, this.GroupKey);
}
