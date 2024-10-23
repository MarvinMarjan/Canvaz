using System.Collections.Generic;

using Canvaz.Language.Definitions;
using Canvaz.Language.Exceptions;
using Canvaz.Language.Typing;


namespace Canvaz.Language.Tools;


public class Environment
{
    private Dictionary<string, Type> _data = [];
    public Dictionary<string, StructureDeclarationStatement> Structures { get; init; } = [];


    public Environment? Enclosing { get; private set; }


    public Environment(Environment? enclosing = null)
    {
        Enclosing = enclosing;
    }


    public void Add(string name, Type value)
    {
        if (Exists(name))
            throw NewError($"Identifier '{name}' has already been defined.");

        _data.Add(name, value);
    }


    public Type Get(string name)
    {
        if (_data.TryGetValue(name, out Type? value))
            return value;

        if (Enclosing is not null)
            return Enclosing.Get(name);

        throw NewError($"Undefined identifier '{name}'");
    }


    public bool TryGet(string name, out Type? value)
    {
        try
        {
            value = Get(name);
            return true;
        }
        catch
        {
            value = null;
            return false;
        }
    }


    public void Set(string name, object? newValue)
    {
        Type value = Get(name);

        value.Value = newValue;
    }


    public bool Exists(string name)
        => _data.ContainsKey(name);


    private static CanvazLangException NewError(string msg)
        => new(null, msg);
}