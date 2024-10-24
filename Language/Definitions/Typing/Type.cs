using Canvaz.Language.Exceptions;
using Canvaz.Language.Tools;


namespace Canvaz.Language.Definitions.Typing;


public partial class Type
{
    private object? _value;
    public object? Value
    {
        get => _value;
        set
        {
            if (value is null)
            {
                _value = value;
                return;
            }

            TypeName newTypeName = TypeName.Name == "Any" ? new("Any") : TypeName.FromValue(value);

            if (newTypeName != TypeName)
                throw NewError($"Can't convert from \"{newTypeName}\" to \"{TypeName}\".");

            TypeName = newTypeName;
            _value = value;
        }
    }

    public TypeName TypeName { get; private set; }


    public Type(object? value, TypeName? type = null)
    {
        TypeName = type ?? new("Any");
        Value = value;
    }


    public static Type FromTokenTypeLiteral(TokenType tokenType)
        => new(tokenType switch {
            TokenType.True => true,
            TokenType.False => false,
            TokenType.Null => null,

            _ => null
        });


    public override string ToString()
    {
        string text = Value?.ToString() ?? "null";
        string typeName = CurrentValueTypeName();

        if (Interpreter.Current!.Environment.Structures.ContainsKey(typeName))
            text = $"Structure \"{typeName}\"";

        if (typeName == "Function")
            text = $"Function \"{(Value as Function)!.Name}\"";

        text = text is "True" or "False" ? text.ToLower() : text; // booleans should be on lower case.

        return text;
    }


    public string CurrentValueTypeName()
    {
        if (Value is null)
            return TypeName.Name;

        // tell the current type instead of "Any"
        if (TypeName.Name == "Any")
            return TypeName.FromValue(Value).Name;

        return TypeName.Name;
    }


    public static bool BothOfType(TypeName left, TypeName right, TypeName target)
        => left == target && right == target;


    public T As<T>() => (T)Value!;


    public bool IsNull() => Value is null;

    public string AsString() => (string)Value!;
    public bool AsBoolean() => (bool)Value!;

    public float AsFloat() => (float)Value!;
    public float AsInteger() => (int)Value!;
    public float AsUInteger() => (uint)Value!;


    private static CanvazLangException NewError(string message)
        => new(null, message);
}