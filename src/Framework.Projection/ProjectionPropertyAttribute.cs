using System;
using System.ComponentModel;

namespace Framework.Projection
{
    /// <summary>
    /// Атрибут марркировки свойсва проекции
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProjectionPropertyAttribute : Attribute
    {
        public ProjectionPropertyAttribute(ProjectionPropertyRole role)
        {
            if (!Enum.IsDefined(typeof(ProjectionPropertyRole), role)) throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(ProjectionPropertyRole));

            this.Role = role;
        }


        /// <summary>
        /// Роль свойства проекции
        /// </summary>
        public ProjectionPropertyRole Role { get; }
    }
}
