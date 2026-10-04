using System.Reflection;

namespace ChipsPocket.Api.Realtime;

public static class RealtimeSchemaGenerator
{
    public static object Generate(Type type)
    {
        return GenerateSchema(type, new HashSet<Type>());
    }

    private static object GenerateSchema(
        Type type,
        HashSet<Type> visited)
    {
        var nullableType = Nullable.GetUnderlyingType(type);

        if (nullableType is not null)
        {
            var schema = GenerateSchema(nullableType, visited);

            if (schema is Dictionary<string, object> dictionary) dictionary["nullable"] = true;

            return schema;
        }

        if (type == typeof(string))
            return Object("string");

        if (type == typeof(Guid))
            return Object("string", "uuid");

        if (type == typeof(DateTime) ||
            type == typeof(DateTimeOffset))
            return Object("string", "date-time");

        if (type == typeof(bool))
            return Object("boolean");

        if (type == typeof(byte) ||
            type == typeof(short) ||
            type == typeof(int) ||
            type == typeof(long) ||
            type == typeof(sbyte) ||
            type == typeof(ushort) ||
            type == typeof(uint) ||
            type == typeof(ulong))
            return Object("integer");

        if (type == typeof(float) ||
            type == typeof(double) ||
            type == typeof(decimal))
            return Object("number");

        if (type.IsEnum)
            return new Dictionary<string, object>
            {
                ["type"] = "string",
                ["enum"] = Enum.GetNames(type)
            };

        if (type.IsArray)
            return new Dictionary<string, object>
            {
                ["type"] = "array",
                ["items"] = GenerateSchema(
                    type.GetElementType()!,
                    visited)
            };

        if (IsDictionary(type))
        {
            var valueType = type.GetGenericArguments()[1];

            return new Dictionary<string, object>
            {
                ["type"] = "object",
                ["additionalProperties"] =
                    GenerateSchema(valueType, visited)
            };
        }

        if (IsEnumerable(type))
        {
            var elementType = type.GetGenericArguments()[0];

            return new Dictionary<string, object>
            {
                ["type"] = "array",
                ["items"] =
                    GenerateSchema(elementType, visited)
            };
        }

        if (!visited.Add(type))
            return new Dictionary<string, object>
            {
                ["type"] = "object"
            };

        var properties = new Dictionary<string, object>();
        var required = new List<string>();

        foreach (var property in type.GetProperties(
                     BindingFlags.Public |
                     BindingFlags.Instance))
        {
            if (!property.CanRead)
                continue;

            properties[property.Name] =
                GenerateSchema(property.PropertyType, visited);

            if (IsRequired(property)) required.Add(property.Name);
        }

        visited.Remove(type);

        var result = new Dictionary<string, object>
        {
            ["type"] = "object",
            ["properties"] = properties
        };

        if (required.Count > 0) result["required"] = required;

        return result;
    }

    private static bool IsRequired(PropertyInfo property)
    {
        var type = property.PropertyType;

        if (type.IsValueType &&
            Nullable.GetUnderlyingType(type) is null)
            return true;

        return property.GetCustomAttribute<RealtimeRequiredAttribute>() is not null;
    }

    private static bool IsEnumerable(Type type)
    {
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }

    private static bool IsDictionary(Type type)
    {
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
    }

    private static Dictionary<string, object> Object(
        string type,
        string? format = null)
    {
        var result = new Dictionary<string, object>
        {
            ["type"] = type
        };

        if (format is not null) result["format"] = format;

        return result;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class RealtimeRequiredAttribute : Attribute;