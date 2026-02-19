namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
//[Obsolete("Use Framework.Restriction.UniqueGroup")]
public class UniqueCollectionValidatorAttribute : PropertyValidatorAttribute
{
    public string GroupKey { get; set; }


    public override IPropertyValidator CreateValidator()
    {
        return new UniqueCollectionValidator (this.GroupKey);
    }
}
