using System.Linq;

using Canvaz.Language.Definitions;
using Canvaz.Language.Exceptions;
using Canvaz.Language.Tools;


namespace Canvaz.Language.Typing;


public class TypeName
{
    public static string[] Primitives { get; private set; } = [
        "String", "Boolean", "Float", "Integer", "UInteger"
    ];


    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (!TypeExists(value))
                throw InvalidType(value);

            _name = value;
        }
    }

    public bool Null { get; set; }


    public TypeName()
    {
        Null = true;
    }

    public TypeName(string name)
    {
        Name = name;
    }


    public bool Is<T>() => this == FromGeneric<T>();


    public static bool operator==(TypeName left, TypeName right) => left.Equals(right);
    public static bool operator!=(TypeName left, TypeName right) => !left.Equals(right);


    public bool Equals(TypeName other)
        => Name == other.Name;

    public override bool Equals(object? obj)
        => obj is TypeName typeName && Equals(typeName);

    public override int GetHashCode()
        => Name.Length * Name.Length;

    public override string ToString()
        => Null ? "Null" : Name;



    public static CanvazLangException InvalidType(string name)
        => new(null, $"Type '{name}' doesn't exists.");


    public static bool TypeExists(string name)
    {
        if (Primitives.Contains(name))
            return true;

        return Interpreter.Current?.Structures.ContainsKey(name) ?? false;
    }


    public static TypeName FromValue(object? value) => value switch
    {
        null => new(),

        string => new("String"),
        bool => new("Boolean"),

        float => new("Float"),
        int => new("Integer"),
        uint => new("UInteger"),

        StructureDeclarationStatement structure => new(structure.Name.Lexeme),

        _ => throw InvalidType(value.GetType().Name)
    };


    public static TypeName FromGeneric<T>()
    {
        if (typeof(T) == typeof(string))
            return new("String");

        if (typeof(T) == typeof(bool))
            return new("Boolean");


        if (typeof(T) == typeof(float))
            return new("Float");

        if (typeof(T) == typeof(int))
            return new("Integer");

        if (typeof(T) == typeof(uint))
            return new("UInteger");


        throw InvalidType(typeof(T).Name);
    }
}