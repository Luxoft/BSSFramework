namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequiredGroupValidatorAttribute(RequiredGroupValidatorMode mode) : ClassValidatorAttribute
{
    public required string GroupKey { get; set; }


    public override IClassValidator CreateValidator(IServiceProvider serviceProvider)
    {
        return new RequiredGroupValidator(mode, this.GroupKey);
    }
}
