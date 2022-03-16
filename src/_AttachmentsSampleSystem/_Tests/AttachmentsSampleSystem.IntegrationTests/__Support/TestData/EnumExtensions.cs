using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.TestData
{
    public static class EnumExtensions
    {
        public static string GetName(this Enum @enum)
        {
            var type = @enum.GetType();
            var name = Enum.GetName(type, @enum);
            if (name == null)
            {
                return string.Empty;
            }

            var customAttribute = type.GetField(name)?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute?.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        public static IEnumerable<string> GetNames<TEnum>(this IEnumerable<TEnum> enums)
            where TEnum : Enum =>
            enums?.Select(e => GetName(e));
    }
}
