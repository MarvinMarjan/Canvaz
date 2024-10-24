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


public class Function(FunctionDeclarationStatement declaration) : ICallable
{
    public FunctionDeclarationStatement Declaration { get; init; } = declaration;

    public TypeName? ReturnType { get; init; } = declaration.ReturnType is Token valid ? new(valid.Lexeme) : null;


    public Type Call(Interpreter interpreter, List<Type> arguments)
    {
        Environment newEnvironment = new(interpreter.Environment);
    
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            VarDeclarationStatement parameterDeclaration = Declaration.Parameters[i];
            Type argument = MatchArgumentWithParameter(interpreter, parameterDeclaration, i >= arguments.Count ? null : arguments[i]);
            
            newEnvironment.Add(parameterDeclaration.Name.Lexeme, argument);
        }

        Type? returnValue = ReturnType is not null ? new(null, ReturnType) : null;

        try
        {
            interpreter.Interpret(Declaration.Body, newEnvironment);
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
        => (from parameter in Declaration.Parameters
                where parameter.Value is not null
                select parameter).Count();


    public int Arity => Declaration.Parameters.Count - CountParametersWithDefaultValues();
}