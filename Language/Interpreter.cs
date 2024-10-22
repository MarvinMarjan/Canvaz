using System;
using System.Collections.Generic;

using Canvaz.Language.Exceptions;


using Type = Canvaz.Language.Typing.Type;


namespace Canvaz.Language;


public class Interpreter : IExpressionProcessor<Type>, IStatementProcessor<object?>
{
    public Dictionary<string, Type> Variables { get; init; } = [];


    public void Interpret(Statement statement)
        => statement.Process(this);

    public void Interpret(List<Statement> statements)
    {
        foreach (Statement statement in statements)
            statement.Process(this);
    }

    public Type Interpret(Expression expression)
        => expression.Process(this);




    public object? ProcessExpressionStatement(ExpressionStatement statement)
    {
        Interpret(statement.Expression);
        return null;
    }

    public object? ProcessBlockStatement(BlockStatement statement)
    {
        Interpret(statement.Statements);
        return null;
    }

    public object? ProcessPrintStatement(PrintStatement statement)
    {
        Console.WriteLine(Interpret(statement.Value)?.ToString());
        return null;
    }

    public object? ProcessVarDeclarationStatement(VarDeclarationStatement statement)
    {
        if (Variables.ContainsKey(statement.Name.Lexeme))
            throw NewError("Variable already declared.", statement.Name);

        Variables.Add(statement.Name.Lexeme, Interpret(statement.Value));
        return null;
    }

    public object? ProcessIfElseStatement(IfElseStatement statement)
    {
        if (Interpret(statement.Condition).IsTruthy())
            Interpret(statement.ThenStatement);

        else if (statement.ElseStatement is not null)
            Interpret(statement.ElseStatement);

        return null;
    }

    public object? ProcessWhileStatement(WhileStatement statement)
    {
        while (Interpret(statement.Condition).IsTruthy())
            Interpret(statement.BlockStatement);

        return null;
    }




    public Type ProcessLiteralExpression(LiteralExpression expression)
        => expression.Value;

    public Type ProcessIdentifierExpression(IdentifierExpression expression)
    {
        CanvazLanguage.CurrentRuntimeTokenReference = expression.Identifier;

        string identifierName = expression.Identifier.Lexeme;

        if (!Variables.TryGetValue(identifierName, out Type? value))
            throw NewError("Undefined identifier.", expression.Identifier);

        return value;
    }

    // TODO: convert all in-string ' character to "

    public Type ProcessAssignmentExpression(AssignmentExpression expression)
    {
        CanvazLanguage.CurrentRuntimeTokenReference = expression.EqualSign;

        if (!Variables.ContainsKey(expression.Identifier.Lexeme))
            throw NewError($"Not defined identifier '{expression.Identifier.Lexeme}'.", expression.Identifier);

        return Variables[expression.Identifier.Lexeme] = Interpret(expression.Value);
    }

    public Type ProcessBinaryExpression(BinaryExpression expression)
    {
        Type left = Interpret(expression.Left);
        Type right = Interpret(expression.Right);

        CanvazLanguage.CurrentRuntimeTokenReference = expression.Operator;

        return expression.Operator.Type switch
        {
            TokenType.Plus => left + right,
            TokenType.Minus => left - right,
            TokenType.Asterisk => left * right,
            TokenType.Slash => left / right,

            TokenType.EqualEqual => left == right,
            TokenType.BangEqual => left != right,

            TokenType.Greater => left > right,
            TokenType.GreaterEqual => left >= right,
            TokenType.Less => left < right,
            TokenType.LessEqual => left <= right,

            TokenType.And => left && right,
            TokenType.Or => left || right,

            _ => throw NewError("Invalid binary expression.", expression.Operator),
        };
    }

    public Type ProcessUnaryExpression(UnaryExpression expression)
    {
        Type right = Interpret(expression.Right);

        CanvazLanguage.CurrentRuntimeTokenReference = expression.Operator;

        return expression.Operator.Type switch
        {
            TokenType.Minus => -right,
            TokenType.Not => !right,
            
            _ => throw NewError("Invalid unary expression.", expression.Operator),
        };
    }
    
    public Type ProcessGroupingExpression(GroupingExpression expression)
        => Interpret(expression.Expression);


    private static CanvazLangException NewError(string message, Token? token = null)
        => new(token is null ? null : new(token!.Value), message);
}