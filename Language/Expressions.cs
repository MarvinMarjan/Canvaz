using Canvaz.Language.Typing;


namespace Canvaz.Language;


public interface IExpressionProcessor<T>
{
    public T ProcessLiteralExpression(LiteralExpression expression);
    public T ProcessIdentifierExpression(IdentifierExpression expression);
    public T ProcessAssignmentExpression(AssignmentExpression expression);
    public T ProcessBinaryExpression(BinaryExpression expression);
    public T ProcessUnaryExpression(UnaryExpression expression);
    public T ProcessGroupingExpression(GroupingExpression expression);
}


public abstract class Expression
{
    public abstract T Process<T>(IExpressionProcessor<T> processor);
}


public class LiteralExpression(Type value) : Expression
{
    public Type Value { get; set; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessLiteralExpression(this);
}


public class IdentifierExpression(Token identifier) : Expression
{
    public Token Identifier { get; set; } = identifier;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessIdentifierExpression(this);
}


public class AssignmentExpression(Token identifier, Expression value) : Expression
{
    public Token Identifier { get; set; } = identifier;
    public Expression Value { get; set; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessAssignmentExpression(this);
}


public class BinaryExpression(Expression left, Token @operator, Expression right) : Expression
{
    public Expression Left { get; set; } = left;
    public Token Operator { get; set; } = @operator;
    public Expression Right { get; set; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessBinaryExpression(this);
}


public class UnaryExpression(Token @operator, Expression right) : Expression
{
    public Token Operator { get; set; } = @operator;
    public Expression Right { get; set; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessUnaryExpression(this);
}


public class GroupingExpression(Expression expression) : Expression
{
    public Expression Expression { get; set; } = expression;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessGroupingExpression(this);
}
