using Canvaz.Language.Exceptions;


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

            TypeName newTypeName = TypeName.FromValue(value);

            if (StaticTyping && newTypeName != TypeName)
                throw NewError($"Can't assign a static typed variable of type '{TypeName}' a value of type '{newTypeName}'.");

            TypeName = newTypeName;
            _value = value;
        }
    }

    public TypeName TypeName { get; private set; } = new();
    public bool StaticTyping { get; set; }


    public Type(object? value)
    {
        StaticTyping = false;
        Value = value;
    }

    public Type(object? value, TypeName type)
    {
        StaticTyping = true;
        TypeName = type;
        Value = value;
    }


    public static Type FromTokenTypeLiteral(TokenType tokenType)
        => new(tokenType switch {
            TokenType.True => true,
            TokenType.False => false,
            TokenType.Null => null,

            _ => null
        }) { StaticTyping = true };


    public override string ToString()
    {
        string text = Value?.ToString() ?? "null";
        text = text is "True" or "False" ? text.ToLower() : text; // booleans should be on lower case.

        return text;
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