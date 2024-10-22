using System.Collections.Generic;

namespace Canvaz.Language;


public interface IStatementProcessor<T>
{
    public T ProcessExpressionStatement(ExpressionStatement statement);
    public T ProcessBlockStatement(BlockStatement statement);
    public T ProcessPrintStatement(PrintStatement statement);
    public T ProcessVarDeclarationStatement(VarDeclarationStatement statement);
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
    public List<Statement> Statements { get; set; } = statements;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessBlockStatement(this);
}


public class PrintStatement(Expression value) : Statement
{
    public Expression Value { get; init; } = value;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessPrintStatement(this);
}


public class VarDeclarationStatement(Token name, Expression value) : Statement
{
    public Token Name { get; init; } = name;
    public Expression Value { get; init; } = value;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessVarDeclarationStatement(this);
}


public class IfElseStatement(Expression condition, Statement thenStatement, Statement? elseStatement = null) : Statement
{
    public Expression Condition { get; set; } = condition;
    public Statement ThenStatement { get; set; } = thenStatement;
    public Statement? ElseStatement { get; set; } = elseStatement;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessIfElseStatement(this);
}


public class WhileStatement(Expression condition, Statement blockStatement) : Statement
{
    public Expression Condition { get; set; } = condition;
    public Statement BlockStatement { get; set; } = blockStatement;


    public override T Process<T>(IStatementProcessor<T> processor)
        => processor.ProcessWhileStatement(this);
}