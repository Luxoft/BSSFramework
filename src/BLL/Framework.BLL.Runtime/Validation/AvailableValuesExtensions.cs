namespace Framework.BLL.Validation;

public static class AvailableValuesExtensions
{
    public static Framework.Validation.AvailableValues ToValidation(this AvailableValues availableValues)
    {
        if (availableValues == null) throw new ArgumentNullException(nameof(availableValues));

        return new Framework.Validation.AvailableValues(availableValues);
    }
}
