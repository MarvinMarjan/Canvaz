using System;
using System.Collections.Generic;
using System.Linq;

using Canvaz.Engine;

using Canvaz.Language.Definitions;
using Canvaz.Language.Definitions.Typing;
using Canvaz.Language.Exceptions;


using Type = Canvaz.Language.Definitions.Typing.Type;


namespace Canvaz.Language.Tools;


public class Interpreter : IExpressionProcessor<Type>, IStatementProcessor<object?>
{
    public static Interpreter Current { get; private set; } = new();

    public Environment Environment { get; private set; }


    public EngineApp? Engine { get; private set; }


    private Interpreter()
    {
        Current = this;

        Environment = new();
        StdLibrary.AddLibraryTo(Environment);
    }


    public void Start(string title)
    {
        Engine = new(title, new());
    }

    public void UpdateAndDraw()
    {
        Engine?.Update();
        Engine?.Draw();
    }


    public void Interpret(Statement statement)
        => statement.Process(this);

    public void Interpret(List<Statement> statements, Environment? environment = null)
    {
        Environment previous = Environment;
        Environment = environment ?? new();

        try
        {
            foreach (Statement statement in statements)
            statement.Process(this);
        }
        finally
        {
            Environment = previous;
        }
    }

    public Type Interpret(Expression expression)
        => expression.Process(this);




    public object? ProcessInternalStatement(InternalStatement statement)
    {
        statement.Action();
        return null;
    }


    public object? ProcessExpressionStatement(ExpressionStatement statement)
    {
        Interpret(statement.Expression);
        return null;
    }


    public object? ProcessBlockStatement(BlockStatement statement)
    {
        Interpret(statement.Statements, new(Environment));
        return null;
    }


    public object? ProcessPrintStatement(PrintStatement statement)
    {
        Console.WriteLine(Interpret(statement.Value)?.ToString());
        return null;
    }


    public object? ProcessVarDeclarationStatement(VarDeclarationStatement statement)
    {
        if (Environment.Exists(statement.Name.Lexeme))
            throw NewError("Identifier has already been declared.", statement.Name);

        Type variable;
        object? value = statement.Value is Expression validValue ? Interpret(validValue).Value : null;

        CanvazLanguage.CurrentRuntimeTokenReference = statement.EqualSign;

        if (statement.TypeName is Token typeName)
            variable = new(value, new(typeName.Lexeme));
        else
            variable = new(value);

        Environment.Add(statement.Name.Lexeme, variable);
        return null;
    }


    public object? ProcessFunctionDeclarationStatement(FunctionDeclarationStatement statement)
    {
        Environment.Add(statement.Name.Lexeme, new(Function.FromStatement(statement)));
        return null;
    }


    public object? ProcessStructureDeclarationStatement(StructureDeclarationStatement statement)
    {
        Environment.AddStructure(statement.Name.Lexeme, statement.Simplify());

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


    public object? ProcessReturnStatement(ReturnStatement statement)
        => throw new Return(Interpret(statement.Value), statement.Keyword);




    public Type ProcessLiteralExpression(LiteralExpression expression)
        => expression.Value;


    public Type ProcessIdentifierExpression(IdentifierExpression expression)
    {
        CanvazLanguage.CurrentRuntimeTokenReference = expression.Identifier;

        string identifierName = expression.Identifier.Lexeme;

        if (!Environment.TryGet(identifierName, out Type? value))
            throw NewError("Undefined identifier.", expression.Identifier);

        return value!;
    }


    public Type ProcessAssignmentExpression(AssignmentExpression expression)
    {
        CanvazLanguage.CurrentRuntimeTokenReference = expression.EqualSign;

        if (!Environment.TryGet(expression.Identifier.Lexeme, out Type? value))
            throw NewError($"Not defined identifier \"{expression.Identifier.Lexeme}\".", expression.Identifier);

        value!.Value = Interpret(expression.Value).Value;

        return value;
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
            TokenType.Typeof => new(right.CurrentValueTypeName()),
            
            _ => throw NewError("Invalid unary expression.", expression.Operator),
        };
    }


    // TODO: improve token tracking system.

    
    public Type ProcessGroupingExpression(GroupingExpression expression)
        => Interpret(expression.Expression);


    public Type ProcessCallExpression(CallExpression expression)
    {
        Type callee = Interpret(expression.Callee);
        List<Type> arguments = (from argument in expression.Arguments select Interpret(argument)).ToList();
    
        CanvazLanguage.CurrentRuntimeTokenReference = expression.Paren;

        if (callee.Value is not ICallable function)
            throw NewError("Can't call a non-callable value.");

        if (arguments.Count < function.Arity)
            throw NewError($"Expected {function.Arity} arguments, got {arguments.Count}.");

        return function.Call(this, arguments);
    }


    public Type ProcessGetExpression(GetExpression expression)
    {
        if (Interpret(expression.Object).Value is not Structure structure)
            throw NewError("Can't get members of a non-structure value.");

        if (!structure.Members.TryGetValue(expression.Member.Lexeme, out Type? value))
            throw NewError($"Member \"{expression.Member.Lexeme}\" not defined.", expression.Member);

        CanvazLanguage.CurrentRuntimeTokenReference = expression.Member;

        return value;
    }


    public Type ProcessSetExpression(SetExpression expression)
    {
        if (Interpret(expression.Object).Value is not Structure structure)
            throw NewError("Can't set members values of a non-structure value.");

        if (!structure.Members.TryGetValue(expression.Member.Lexeme, out _))
            throw NewError($"Member \"{expression.Member.Lexeme}\" not defined.", expression.Member);

        CanvazLanguage.CurrentRuntimeTokenReference = expression.EqualSign;

        Type value = Interpret(expression.Value);
        Type member = structure.Members[expression.Member.Lexeme];
        member.Value = value.Value;

        return member;
    }


    public Type ProcessStructureInitializationExpression(StructureInitializationExpression expression)
    {
        Token structureName = expression.StructureName;

        if (!Environment.TryGetStructure(expression.StructureName.Lexeme, out StructureDeclaration? structureDeclaration))
            throw NewError($"Structure \"{structureName.Lexeme}\" is not defined.", structureName);

        Structure structure = new(structureName.Lexeme);

        foreach (var (name, varDeclaration) in structureDeclaration!.Value.Members)
        {
            if (!expression.InitializationPairs.TryGetValue(name, out StructureInitializationPair init))
            {
                if (varDeclaration.Value is null)
                    throw NewError($"Structure member \"{name}\" must be initialized.", structureName);
                else
                    structure.Members.Add(name, new(Interpret(varDeclaration.Value).Value));
            }
            else
                structure.Members.Add(name, new(Interpret(init.Value).Value, varDeclaration.TypeName));
        }

        return new(structure);
    }


    public Type ProcessAnonymousFunctionExpression(AnonymousFunctionExpression expression)
        => new(Function.FromExpression(expression));


    private static CanvazLangException NewError(string message, Token? token = null)
        => new(token is null ? null : new(token!.Value), message);
}