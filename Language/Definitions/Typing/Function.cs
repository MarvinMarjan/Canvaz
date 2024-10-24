using System;
using System.Collections.Generic;
using System.Linq;

using Canvaz.Language.Exceptions;
using Canvaz.Language.Tools;


using Environment = Canvaz.Language.Tools.Environment;


namespace Canvaz.Language.Definitions.Typing;


public class Return(Type value, Token keyword) : Exception
{
    public Type Value { get; init; } = value;
    public Token Keyword { get; init; } = keyword;
}


public class Function(string name, List<VarDeclarationStatement> parameters, Token? returnTypeToken, List<Statement> body) : ICallable
{
    public string Name { get; init; } = name;
    public List<VarDeclarationStatement> Parameters { get; init; } = parameters;
    public Token? ReturnTypeToken { get; init; } = returnTypeToken;
    public List<Statement> Body { get; init; } = body;


    public TypeName? ReturnType { get; init; } = returnTypeToken is Token valid ? new(valid.Lexeme) : null;


    public Type Call(Interpreter interpreter, List<Type> arguments)
    {
        Environment newEnvironment = new(interpreter.Environment);
    
        for (int i = 0; i < Parameters.Count; i++)
        {
            VarDeclarationStatement parameterDeclaration = Parameters[i];
            Type argument = MatchArgumentWithParameter(interpreter, parameterDeclaration, i >= arguments.Count ? null : arguments[i]);
            
            newEnvironment.Add(parameterDeclaration.Name.Lexeme, argument);
        }

        Type? returnValue = ReturnType is not null ? new(null, ReturnType) : null;

        try
        {
            interpreter.Interpret(Body, newEnvironment);
        }
        catch (Return ret)
        {
            if (returnValue is null)
                throw new CanvazLangException(ret.Keyword, "Can't return from a function without return type.");

            returnValue.Value = ret.Value.Value;
        }

        return returnValue ?? new(null);
    }


    private static Type MatchArgumentWithParameter(Interpreter interpreter, VarDeclarationStatement parameterDeclaration, Type? argument)
    {
        Type parameter;
        object? value;

        if (argument is null) // no need to check nullability of "Value"
            value = interpreter.Interpret(parameterDeclaration.Value!).Value;
        else
            value = argument.Value;

        if (parameterDeclaration.TypeName is Token validTypeName)
            parameter = new(value, new(validTypeName.Lexeme));
        else
            parameter = new(value);
    
        return parameter;
    }


    private int CountParametersWithDefaultValues()
        => (from parameter in Parameters
                where parameter.Value is not null
                select parameter).Count();


    public int Arity => Parameters.Count - CountParametersWithDefaultValues();
}