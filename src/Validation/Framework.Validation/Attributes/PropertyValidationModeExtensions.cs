namespace Framework.Validation.Attributes;

internal static class PropertyValidationModeExtensions
{
    public static PropertyValidationMode ToPropertyValidationMode(this bool value) => value ? PropertyValidationMode.Enabled : PropertyValidationMode.Disabled;

    public static PropertyValidationMode ToPropertyValidationMode(this bool? value)
    {
        switch (value)
        {
            case true:
                return PropertyValidationMode.Enabled;

            case false:
                return PropertyValidationMode.Disabled;

            default:
                return PropertyValidationMode.Auto;
        }
    }
}
