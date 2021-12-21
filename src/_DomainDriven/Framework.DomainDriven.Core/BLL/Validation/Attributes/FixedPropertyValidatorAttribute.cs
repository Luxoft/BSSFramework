using System;

using Framework.Validation;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Атрибут для проверки неизменяемости свойства
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FixedPropertyValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return new FixedPropertyValidator();
        }
    }
}
