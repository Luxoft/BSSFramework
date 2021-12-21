using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Restriction
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : Attribute, IRestrictionAttribute
    {
        public RequiredMode Mode { get; set; }
    }

    /// <summary>
    /// Дополнительные режимы валидации
    /// </summary>
    public enum RequiredMode
    {
        Default,

        /// <summary>
        /// Разрешает пустую строку, идёт проверка только на null-значение
        /// </summary>
        AllowEmptyString,

        /// <summary>
        /// Требует, чтобы период был строго закрыт, т.е. Period.EndDate != null
        /// </summary>
        ClosedPeriodEndDate
    }

    public static class RequiredModeExtensions
    {
        public static void ValidateAppliedType(this RequiredMode requiredMode, [NotNull] Type appliedType)
        {
            if (appliedType == null) throw new ArgumentNullException(nameof(appliedType));

            if (!requiredMode.IsValidAppliedType(appliedType))
            {
                throw new Exception($"{requiredMode.ToCSharpCode()} can't be applied to type {appliedType}");
            }
        }

        public static bool IsValidAppliedType(this RequiredMode requiredMode, [NotNull] Type appliedType)
        {
            if (appliedType == null) throw new ArgumentNullException(nameof(appliedType));

            switch (requiredMode)
            {
                case RequiredMode.AllowEmptyString:

                    return typeof (string) == appliedType;

                case RequiredMode.ClosedPeriodEndDate:

                    return typeof(Period) == appliedType;

                case RequiredMode.Default:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(requiredMode));
            }
        }
    }
}
