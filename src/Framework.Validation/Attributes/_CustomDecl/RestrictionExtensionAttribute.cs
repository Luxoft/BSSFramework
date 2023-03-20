using System;

using Framework.Restriction;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public class RestrictionExtensionAttribute : Attribute, IValidationData
{
    public RestrictionExtensionAttribute(Type attributeType)
    {
        if (attributeType == null) throw new ArgumentNullException(nameof(attributeType));

        if (!typeof(IRestrictionAttribute).IsAssignableFrom(attributeType))
        {
            throw new ArgumentOutOfRangeException(nameof(attributeType));
        }

        this.AttributeType = attributeType;
        this.OperationContext = int.MaxValue;
    }


    public Type AttributeType { get; private set; }


    public int OperationContext { get; set; }

    public object CustomError { get; set; }
}
