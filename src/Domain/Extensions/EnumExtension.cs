using System.ComponentModel;
using System.Reflection;

namespace Domain.Extensions;

public static class EnumExtension
{
    public static string GetDescription<T>(this T enumValue) where T : Enum
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? enumValue.ToString();
    }
}