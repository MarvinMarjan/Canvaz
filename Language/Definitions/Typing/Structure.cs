using System.Collections.Generic;


namespace Canvaz.Language.Definitions.Typing;


public class Structure(string name)
{
    public string Name { get; set; } = name;
    public Dictionary<string, Type> Members { get; init; } = [];
}


public record struct StructureInitializationPair(Token Name, Expression Value);