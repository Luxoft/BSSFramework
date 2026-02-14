using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Validation;

public static class CustomAttributeProviderExtensions
{
    extension(ICustomAttributeProvider attributeProvider)
    {
        public IEnumerable<TResult> TryGetRestrictionValidatorAttribute<TRestrictionAttribute, TResult>(Func<TRestrictionAttribute, TResult> getValidatorAttribute)
            where TRestrictionAttribute : Attribute, IRestrictionAttribute
            where TResult : ValidatorAttribute
        {
            if (attributeProvider == null) throw new ArgumentNullException(nameof(attributeProvider));
            if (getValidatorAttribute == null) throw new ArgumentNullException(nameof(getValidatorAttribute));

            var extensionAttrLazy = LazyHelper.Create(() => attributeProvider.GetCustomAttribute<RestrictionExtensionAttribute>(attr => attr.AttributeType == typeof(TRestrictionAttribute)));

            foreach (var restrictionAttr in attributeProvider.GetCustomAttributes<TRestrictionAttribute>())
            {
                var attr = getValidatorAttribute(restrictionAttr);

                if (extensionAttrLazy.Value != null)
                {
                    attr.OperationContext = extensionAttrLazy.Value.OperationContext;
                    attr.CustomError = extensionAttrLazy.Value.CustomError;
                }

                yield return attr;
            }
        }

        public IEnumerable<PropertyValidatorAttribute> TryGetRestrictionValidatorAttributes()
        {
            var p1 = attributeProvider.TryGetRestrictionValidatorAttribute<RequiredAttribute, PropertyValidatorAttribute>(attr => new RequiredValidatorAttribute { Mode = attr.Mode });

            var p2 = attributeProvider.TryGetRestrictionValidatorAttribute<MaxLengthAttribute, PropertyValidatorAttribute>(attr => new MaxLengthValidatorAttribute { MaxLength = attr.Value });

            var p3 = attributeProvider.TryGetRestrictionValidatorAttribute<UniqueGroupAttribute, PropertyValidatorAttribute>(attr => new UniqueCollectionValidatorAttribute { GroupKey = attr.Key });

            return p1.Concat(p2).Concat(p3);
        }
    }
}
