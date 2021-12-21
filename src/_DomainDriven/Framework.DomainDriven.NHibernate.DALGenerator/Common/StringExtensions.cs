using System.Linq;

namespace Framework.DomainDriven.Common
{
    public static class StringExtensions
    {
        public static string ToPropertyName(this string fieldName)
        {
            if (fieldName.StartsWith("_"))
            {
                fieldName = new string(fieldName.Skip(1).ToArray());
            }
            var chars = fieldName.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}
