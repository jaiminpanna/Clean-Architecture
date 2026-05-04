using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace CleanArchitecture.Domain.Utility
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumValue, params object[] replacements) where T : struct
        {
            MemberInfo[] member = enumValue.GetType().GetMember(enumValue.ToString() ?? string.Empty);
            if (member.Length != 0)
            {
                object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
                if (customAttributes.Length != 0)
                {
                    return string.Format(CultureInfo.InvariantCulture, ((DescriptionAttribute)customAttributes[0]).Description, replacements);
                }
            }

            return enumValue.ToString() ?? string.Empty;
        }
    }
}
