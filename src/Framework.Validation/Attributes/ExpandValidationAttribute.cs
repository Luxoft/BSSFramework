using System;

namespace Framework.Validation
{
    /// <summary>
    /// Атрибут, указывающий на то, что валидация будет происходить с раскрытием свойств класса
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ExpandValidationAttribute : Attribute
    {
    }
}