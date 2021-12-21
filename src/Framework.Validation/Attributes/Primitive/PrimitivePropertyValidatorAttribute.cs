using System;
using Framework.Core;

namespace Framework.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimitivePropertyValidatorAttribute : PropertyValidatorAttribute
    {
        private static readonly IDictionaryCache<Type, IPropertyValidator> Cache = new DictionaryCache<Type, IPropertyValidator>(
            type => (IPropertyValidator)Activator.CreateInstance(type)).WithLock();


        private readonly Type _validatatorType;


        public PrimitivePropertyValidatorAttribute(Type validatatorType)
        {
            if (validatatorType == null) throw new ArgumentNullException(nameof(validatatorType));

            this._validatatorType = validatatorType;
        }


        public override IPropertyValidator CreateValidator()
        {
            return Cache[this._validatatorType];
        }
    }
}