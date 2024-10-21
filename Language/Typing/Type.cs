using Canvaz.Engine.Types;
using Canvaz.Language.Exceptions;


namespace Canvaz.Language.Typing;


public enum TypeName
{
    Invalid,

    Null,

    String, Boolean,

    Float, Integer, UInteger,

    Vec2f, Vec2i, Vec2u
}


public partial class Type
{
    private object? _value;
    public object? Value
    {
        get => _value;
        set
        {
            TypeName newTypeName = TypeNameFromValue(value);

            if (newTypeName is TypeName.Invalid)
                throw NewError("Invalid type.", true);

            TypeName = newTypeName;
            _value = value;
        }
    }

    public TypeName TypeName { get; private set; }

    public TokenRange? ReferenceToken { get; init; }


    public Type(object? value = null, TokenRange? referenceToken = null)
    {
        Value = value;
        ReferenceToken = referenceToken;
    }


    public static Type FromTokenType(TokenType tokenType, TokenRange? referenceToken = null)
        => new(tokenType switch {
            TokenType.True => true,
            TokenType.False => false,
            TokenType.Null => null,

            _ => null
        }, referenceToken);


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

    public Vec2f AsVec2f() => (Vec2f)Value!;
    public Vec2i AsVec2i() => (Vec2i)Value!;
    public Vec2u AsVec2u() => (Vec2u)Value!;


    public static TypeName TypeNameFromValue(object? value) => value switch
    {
        null => TypeName.Null,

        string => TypeName.String,
        bool => TypeName.Boolean,

        float => TypeName.Float,
        int => TypeName.Integer,
        uint => TypeName.UInteger,

        Vec2f => TypeName.Vec2f,
        Vec2i => TypeName.Vec2i,
        Vec2u => TypeName.Vec2u,

        _ => TypeName.Invalid
    };


    public static TypeName TypeNameFromType<T>()
    {
        if (typeof(T) == typeof(string))
            return TypeName.String;

        if (typeof(T) == typeof(bool))
            return TypeName.Boolean;


        if (typeof(T) == typeof(float))
            return TypeName.Float;

        if (typeof(T) == typeof(int))
            return TypeName.Integer;

        if (typeof(T) == typeof(uint))
            return TypeName.UInteger;


        if (typeof(T) == typeof(Vec2f))
            return TypeName.Vec2f;

        if (typeof(T) == typeof(Vec2i))
            return TypeName.Vec2i;

        if (typeof(T) == typeof(Vec2u))
            return TypeName.Vec2u;


        return TypeName.Invalid;
    }


    private CanvazLangException NewError(string message, bool noReference = false)
        => new(noReference ? null : ReferenceToken, message);
}