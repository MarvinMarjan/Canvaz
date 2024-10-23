using System.Collections.Generic;

using Canvaz.Language.Tools;


namespace Canvaz.Language.Definitions.Typing;


public class Function(FunctionDeclarationStatement declaration) : ICallable
{
    public FunctionDeclarationStatement Declaration { get; init; } = declaration;


    public Type Call(Interpreter interpreter, List<Type> arguments)
    {
        Environment newEnvironment = new(interpreter.Environment);
    
        for (int i = 0; i < Declaration.Parameters.Count; i++)
            newEnvironment.Add(Declaration.Parameters[i].Lexeme, arguments[i]);

        interpreter.Interpret(Declaration.Body, newEnvironment);

        return new(null);
    }


    public int Arity => Declaration.Parameters.Count;
}