using System;

using Framework.Core;

namespace Framework.Validation
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimitiveClassValidatorAttribute : ClassValidatorAttribute
    {
        private static readonly IDictionaryCache<Type, IClassValidator> Cache = new DictionaryCache<Type, IClassValidator>(
            type => (IClassValidator)Activator.CreateInstance(type)).WithLock();


        private readonly Type _validatatorType;


        public PrimitiveClassValidatorAttribute(Type validatatorType)
        {
            if (validatatorType == null) throw new ArgumentNullException(nameof(validatatorType));

            this._validatatorType = validatatorType;
        }


        public override IClassValidator CreateValidator()
        {
            return Cache[this._validatatorType];
        }
    }
}