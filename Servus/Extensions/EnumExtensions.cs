namespace Servus.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                if (attribute?.Description == description)
                    return (T)field.GetValue(null)!;

                if (field.Name == description)
                    return (T)field.GetValue(null)!;
            }

            throw new ArgumentException($"Description '{description}' not found in enum {type.Name}");
        }

        public static List<(T Value, string Description)> GetEnumValuesWithDescriptions<T>() where T : Enum
        {
            var type = typeof(T);
            var values = new List<(T Value, string Description)>();

            foreach (var field in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                var value = (T)field.GetValue(null)!;
                var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                var description = attribute?.Description ?? field.Name;

                values.Add((value, description));
            }

            return values;
        }
    }
}
