using System.Collections.Generic;

using Canvaz.Language.Exceptions;
using Canvaz.Language.Definitions.Typing;
using Canvaz.Language.Definitions;


namespace Canvaz.Language.Tools;


public class Environment
{
    private Dictionary<string, Type> _data = [];
    public Dictionary<string, StructureDeclaration> Structures { get; set; } = [];


    public Environment? Enclosing { get; private set; }


    public Environment(Environment? enclosing = null)
    {
        Enclosing = enclosing;
    }


    public void Add(string name, Type value)
    {
        if (Exists(name))
            throw NewError($"Identifier \"{name}\" has already been defined.");

        _data.Add(name, value);
    }


    public void AddStructure(string name, StructureDeclaration declaration)
    {
        if (ExistsStructure(name))
            throw NewError($"Structure \"{name}\" has already been defined.");

        Structures.Add(name, declaration);
    }


    public void AddStructures(Dictionary<string, StructureDeclaration> structures)
    {
        foreach (var (key, value) in structures)
            AddStructure(key, value);
    }


    public Type Get(string name)
    {
        if (_data.TryGetValue(name, out Type? value))
            return value;

        if (Enclosing is not null)
            return Enclosing.Get(name);

        throw NewError($"Undefined identifier \"{name}\"");
    }


    public StructureDeclaration GetStructure(string name)
    {
        if (Structures.TryGetValue(name, out StructureDeclaration structure))
            return structure;

        if (Enclosing is not null)
            return Enclosing.GetStructure(name);

        throw NewError($"Undefined structure \"{name}\"");
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


    public bool TryGetStructure(string name, out StructureDeclaration? structure)
    {
        try
        {
            structure = GetStructure(name);
            return true;
        }
        catch
        {
            structure = null;
            return false;
        }
    }


    public void Set(string name, object? newValue)
    {
        Type value = Get(name);

        value.Value = newValue;
    }


    public bool Exists(string name)
    {
        bool exists = _data.ContainsKey(name);
    
        if (!exists && Enclosing is not null)
            return Enclosing.Exists(name);

        return exists;
    }
    
    public bool ExistsStructure(string name)
    {
        bool exists = Structures.ContainsKey(name);
    
        if (!exists && Enclosing is not null)
            return Enclosing.ExistsStructure(name);

        return exists;
    }


    private static CanvazLangException NewError(string msg)
        => new(null, msg);
}