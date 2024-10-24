using System.Collections.Generic;

using Canvaz.Language.Definitions.Typing;


namespace Canvaz.Language.Definitions;


public interface IExpressionProcessor<T>
{
    public T ProcessLiteralExpression(LiteralExpression expression);
    public T ProcessIdentifierExpression(IdentifierExpression expression);
    public T ProcessAssignmentExpression(AssignmentExpression expression);
    public T ProcessBinaryExpression(BinaryExpression expression);
    public T ProcessUnaryExpression(UnaryExpression expression);
    public T ProcessGroupingExpression(GroupingExpression expression);
    public T ProcessCallExpression(CallExpression expression);
    public T ProcessGetExpression(GetExpression expression);
    public T ProcessSetExpression(SetExpression expression);
    public T ProcessStructureInitializationExpression(StructureInitializationExpression expression);
}


public abstract class Expression
{
    public abstract T Process<T>(IExpressionProcessor<T> processor);
}


public class LiteralExpression(Type value) : Expression
{
    public Type Value { get; init; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessLiteralExpression(this);
}


public class IdentifierExpression(Token identifier) : Expression
{
    public Token Identifier { get; init; } = identifier;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessIdentifierExpression(this);
}


public class AssignmentExpression(Token identifier, Token equalSign, Expression value) : Expression
{
    public Token Identifier { get; init; } = identifier;
    public Token EqualSign { get; init; } = equalSign;
    public Expression Value { get; init; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessAssignmentExpression(this);
}


public class BinaryExpression(Expression left, Token @operator, Expression right) : Expression
{
    public Expression Left { get; init; } = left;
    public Token Operator { get; init; } = @operator;
    public Expression Right { get; init; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessBinaryExpression(this);
}


public class UnaryExpression(Token @operator, Expression right) : Expression
{
    public Token Operator { get; init; } = @operator;
    public Expression Right { get; init; } = right;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessUnaryExpression(this);
}


public class GroupingExpression(Expression expression) : Expression
{
    public Expression Expression { get; init; } = expression;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessGroupingExpression(this);
}


public class CallExpression(Expression callee, Token paren, List<Expression> arguments) : Expression
{
    public Expression Callee { get; init; } = callee;
    public Token Paren { get; init; } = paren;
    public List<Expression> Arguments { get; init; } = arguments;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessCallExpression(this);
}


public class GetExpression(Expression @object, Token member) : Expression
{
    public Expression Object { get; init; } = @object;
    public Token Member { get; init; } = member;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessGetExpression(this);
}


public class SetExpression(Expression @object, Token member, Token equalSign, Expression value) : Expression
{
    public Expression Object { get; init; } = @object;
    public Token Member { get; init; } = member;
    public Token EqualSign { get; init; } = equalSign;
    public Expression Value { get; init; } = value;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessSetExpression(this);
}


public class StructureInitializationExpression(Token structureName, Dictionary<string, StructureInitializationPair> initializationPairs) : Expression
{
    public Token StructureName { get; init; } = structureName;
    public Dictionary<string, StructureInitializationPair> InitializationPairs { get; init; } = initializationPairs;


    public override T Process<T>(IExpressionProcessor<T> processor)
        => processor.ProcessStructureInitializationExpression(this);
}

