namespace Canvaz.Language;


public interface IStatementProcessor<T>
{
    public T ProcessExpressionStatement(ExpressionStatement statement);
    public T ProcessPrintStatement(PrintStatement statement);
    public T ProcessVarDeclarationStatement(VarDeclarationStatement statement);
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