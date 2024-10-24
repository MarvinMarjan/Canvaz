using System.Collections.Generic;


namespace Canvaz.Language.Definitions.Typing;


public class Structure(string name)
{
    public string Name { get; set; } = name;
    public Dictionary<string, Type> Members { get; init; } = [];
}


public readonly struct StructureDeclaration(string name, Dictionary<string, VarDeclaration> members)
{
    public string Name { get; init; } = name;
    public Dictionary<string, VarDeclaration> Members { get; init; } = members;
}


public record struct StructureInitializationPair(Token Name, Expression Value);