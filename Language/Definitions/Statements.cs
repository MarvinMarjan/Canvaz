using System.Collections.Generic;
using System.Net.Mime;
using Canvaz.Language.Definitions.Typing;


namespace Canvaz.Language.Definitions;


public interface IStatementProcessor<T>
{
    public T ProcessExpressionStatement(ExpressionStatement statement);
    public T ProcessBlockStatement(BlockStatement statement);
    public T ProcessPrintStatement(PrintStatement statement);
    public T ProcessVarDeclarationStatement(VarDeclarationStatement statement);
    public T ProcessFunctionDeclarationStatement(FunctionDeclarationStatement statement);
    public T ProcessReturnStatement(ReturnStatement statement);
    public T ProcessStructureDeclarationStatement(StructureDeclarationStatement statement);
    public T ProcessIfElseStatement(IfElseStatement statement);
    public T ProcessWhileStatement(WhileStatement statement);
}


public abstract class Statement
{
    public abstract T Process<T>(IStatementProcessor<T> processor);
}


public class ExpressionStatement(Expression expression) : Statement
{
    public Expression Expression { get; init; } = expression;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessExpressionStatement(this);
}


public class BlockStatement(List<Statement> statements) : Statement
{
    public List<Statement> Statements { get; init; } = statements;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessBlockStatement(this);
}


public class PrintStatement(Expression value) : Statement
{
    public Expression Value { get; init; } = value;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessPrintStatement(this);
}


public class VarDeclarationStatement(Token name, Token? typeName, Token? equalSign, Expression? value) : Statement
{
    public Token Name { get; init; } = name;
    public Token? TypeName { get; init; } = typeName;
    public Token? EqualSign { get; init; } = equalSign;
    public Expression? Value { get; init; } = value;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessVarDeclarationStatement(this);
}


public class FunctionDeclarationStatement(Token name, List<VarDeclarationStatement> parameters, Token? returnType, List<Statement> body) : Statement
{
    public Token Name { get; init; } = name;
    public List<VarDeclarationStatement> Parameters { get; init; } = parameters;
    public Token? ReturnType { get; init; } = returnType;
    public List<Statement> Body { get; init; } = body;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessFunctionDeclarationStatement(this);
}


public class ReturnStatement(Token keyword, Expression value) : Statement
{
    public Token Keyword { get; init; } = keyword;
    public Expression Value { get; init; } = value;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessReturnStatement(this);
}


public class StructureDeclarationStatement(Token name, Dictionary<string, VarDeclarationStatement> members) : Statement
{
    public Token Name { get; init; } = name;
    public Dictionary<string, VarDeclarationStatement> Members { get; init; } = members;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessStructureDeclarationStatement(this);
}


public class IfElseStatement(Expression condition, Statement thenStatement, Statement? elseStatement = null) : Statement
{
    public Expression Condition { get; init; } = condition;
    public Statement ThenStatement { get; init; } = thenStatement;
    public Statement? ElseStatement { get; init; } = elseStatement;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessIfElseStatement(this);
}


public class WhileStatement(Expression condition, Statement blockStatement) : Statement
{
    public Expression Condition { get; init; } = condition;
    public Statement BlockStatement { get; init; } = blockStatement;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessWhileStatement(this);
}