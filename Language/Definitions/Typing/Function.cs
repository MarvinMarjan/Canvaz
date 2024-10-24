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


public class Function(string name, List<VarDeclaration> parameters, TypeName? returnType, List<Statement> body) : ICallable
{
    public string Name { get; init; } = name;
    public List<VarDeclaration> Parameters { get; init; } = parameters;
    public TypeName? ReturnTypeToken { get; init; } = returnType;
    public List<Statement> Body { get; init; } = body;


    public TypeName? ReturnType { get; init; } = returnType;


    public static Function FromStatement(FunctionDeclarationStatement statement)
    {
        List<VarDeclaration> parameters = (from parameter in statement.Parameters select parameter.Simplify()).ToList();
        TypeName? typeName = null;

        if (statement.ReturnType is not null)
            typeName = statement.ReturnType.Value.Lexeme;

        return new(statement.Name.Lexeme, parameters, typeName, statement.Body);
    }


    public static Function FromExpression(AnonymousFunctionExpression expression)
    {
        List<VarDeclaration> parameters = (from parameter in expression.Parameters select parameter.Simplify()).ToList();
        TypeName? typeName = null;

        if (expression.ReturnType is not null)
            typeName = expression.ReturnType.Value.Lexeme;

        return NewAnonymous(parameters, typeName, expression.Body);
    }


    public static Function NewAnonymous(List<VarDeclaration> parameters, TypeName? returnType, List<Statement> body)
        => new("anonymous", parameters, returnType, body);


    public Type Call(Interpreter interpreter, List<Type> arguments)
    {
        Environment newEnvironment = new(interpreter.Environment);
    
        for (int i = 0; i < Parameters.Count; i++)
        {
            VarDeclaration parameterDeclaration = Parameters[i];
            Type argument = MatchArgumentWithParameter(interpreter, parameterDeclaration, i >= arguments.Count ? null : arguments[i]);
            
            newEnvironment.Add(parameterDeclaration.Name, argument);
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


    private static Type MatchArgumentWithParameter(Interpreter interpreter, VarDeclaration parameterDeclaration, Type? argument)
    {
        object? value;

        // default value
        if (argument is null) // no need to check nullability of "Value"
            value = interpreter.Interpret(parameterDeclaration.Value!).Value;
        else
            value = argument.Value;

        return new(value, parameterDeclaration.TypeName);
    }


    private int CountParametersWithDefaultValues()
        => (from parameter in Parameters
                where parameter.Value is not null
                select parameter).Count();


    public int Arity => Parameters.Count - CountParametersWithDefaultValues();
}