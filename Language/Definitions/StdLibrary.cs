using System.Collections.Generic;

using Canvaz.Language.Definitions.Typing;
using Canvaz.Language.Tools;


using Environment = Canvaz.Language.Tools.Environment;


namespace Canvaz.Language.Definitions;


public static class StdLibrary
{
    private static Type NewFunction(string name, List<VarDeclaration> parameters, TypeName? returnType, List<Statement> body)
        => new(new Function(name, parameters, returnType, body));


    private static KeyValuePair<string, StructureDeclaration> NewStructure(string name, Dictionary<string, VarDeclaration> members)
        => new(name, new(name, members));

    private static KeyValuePair<string, VarDeclaration> NewMember(string name, TypeName typeName, Expression? value)
        => new(name, new(name, typeName, value));

    private static KeyValuePair<string, VarDeclaration> NewFunctionMember(string name, List<VarDeclaration> parameters, TypeName? returnType, List<Statement> body)
        => NewMember(name, "Function", new LiteralExpression(new(Function.NewAnonymous(parameters, returnType, body))));


    public static void AddLibraryTo(Environment environment)
    {
        environment.AddStructures(new([
            NewStructure("Vec2f", new([
                NewMember("x", "Float", null),
                NewMember("y", "Float", null)
            ])),

            NewStructure("Vec2i", new([
                NewMember("x", "Integer", null),
                NewMember("y", "Integer", null)
            ])),

            NewStructure("Vec2u", new([
                NewMember("x", "UInteger", null),
                NewMember("y", "UInteger", null)
            ]))
        ]));


        environment.AddStructures(new([
            NewStructure("EngineSettings", new([
                NewMember("windowTitle", "String", null),
                NewMember("windowSize", "Vec2u", null)
            ]))
        ]));


        environment.Add("start", NewFunction(
            "start",
            [new("settings", "EngineSettings", null)],
            null,
            [new InternalStatement(() => {
                Structure settings = Interpreter.Current.Environment.Get("settings").As<Structure>();
            
                Interpreter.Current.Start(settings.Members["windowTitle"].AsString());
            })]
        ));

        environment.Add("update", NewFunction("update", [], null,
            [
                new InternalStatement(Interpreter.Current.UpdateAndDraw)
            ]
        ));

        environment.Add("isOpen", NewFunction("isOpen", [], "Boolean",
            [
                // TODO: do this
            ]
        ));
    }
}