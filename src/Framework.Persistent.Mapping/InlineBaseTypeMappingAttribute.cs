using System;

namespace Framework.Persistent.Mapping
{
    /// <summary>
    /// Указание, что маппинг по данному объекту будет сгенерён отдельно об базового класса, но с его полями (требует соответствия имени таблицы).
    /// Как независимый объект. т.е. иерархия не соблюдается.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InlineBaseTypeMappingAttribute : Attribute
    {

    }
}
