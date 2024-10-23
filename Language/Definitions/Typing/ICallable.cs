using System.Collections.Generic;

using Canvaz.Language.Tools;


namespace Canvaz.Language.Definitions.Typing;


public interface ICallable
{
    Type Call(Interpreter interpreter, List<Type> arguments);

    int Arity { get; }
}