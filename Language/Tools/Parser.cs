using System;
using System.Collections.Generic;

using Canvaz.Language.Definitions;
using Canvaz.Language.Exceptions;


namespace Canvaz.Language.Tools;


public class Parser
{
    private List<Token> _tokens = [];
    private List<Statement> _statements = [];
    private int _current = 0;


    public List<Statement> Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _statements = [];
        _current = 0;

        if (_tokens.Count == 0)
            throw new ArgumentException("Don't parse a empty token list.");

        while (!AtEnd())
        {
            try
            {
                _statements.Add(Declaration());
            }
            catch (Exception e)
            {
                CanvazLanguage.Error(e);
                Synchronize();
            }
        }

        return _statements;
    }




    private Statement Declaration()
    {
        TokenType tokenType = Advance().Type;

        if (tokenType == TokenType.Var) return VarDeclarationStatement();
        if (tokenType == TokenType.Structure) return StructureDeclarationStatement();

        Retreat();
        return Statement();
    }

    private Statement Statement()
    {
        TokenType tokenType = Advance().Type;
    
        if (tokenType == TokenType.Print) return PrintStatement();
        if (tokenType == TokenType.If) return IfElseStatement();
        if (tokenType == TokenType.While) return WhileStatement();
        if (tokenType == TokenType.BraceLeft) return new BlockStatement(Block());

        Retreat();
        return ExpressionStatement();
    }

    private ExpressionStatement ExpressionStatement()
        => new(Expression());

    private List<Statement> Block()
    {
        List<Statement> statements = [];

        while (!Check(TokenType.BraceRight) && !AtEnd())
            statements.Add(Declaration());

        Expect(TokenType.BraceRight, "Expect '}' to finish a block.");
        return statements;
    }


    private PrintStatement PrintStatement()
        => new(Expression());


    private VarDeclarationStatement VarDeclarationStatement()
    {
        Token name = Expect(TokenType.Identifier, "Variable name expected.");
        Token? typeName = null;
        Expression? value = null;

        if (Match(TokenType.Colon))
            typeName = Expect(TokenType.Identifier, "Type name expected.");

        if (Match(TokenType.Equal))
            value = Expression();

        return new(name, typeName, value);
    }


    private IfElseStatement IfElseStatement()
    {
        Expression condition = Expression();
        Statement thenStatement = Statement();
        Statement? elseStatement = null;

        if (Match(TokenType.Else))
            elseStatement = Statement();

        return new(condition, thenStatement, elseStatement);
    }


    private WhileStatement WhileStatement()
    {
        Expression condition = Expression();
        Statement statement = Statement();

        return new(condition, statement);
    }


    private StructureDeclarationStatement StructureDeclarationStatement()
    {
        Token name = Expect(TokenType.Identifier, "Structure name expected.");
        List<VarDeclarationStatement> varDeclarations = [];

        Expect(TokenType.BraceLeft, "Expect '{' after structure name.");

        while (!Check(TokenType.BraceRight) && !AtEnd())
        {
            Advance();
            varDeclarations.Add(VarDeclarationStatement());
        }

        Expect(TokenType.BraceRight, "Expect '}' after structure body.");
        return new(name, varDeclarations);
    }








    private Expression Expression()
        => Assignment(); 


    private Expression Assignment()
    {
        Expression expression = AndOr();

        if (Match(TokenType.Equal))
        {
            Token equal = Previous();
            Expression value = Assignment();

            if (expression is IdentifierExpression identifierExpression)
            {
                Token name = identifierExpression.Identifier;
                return new AssignmentExpression(name, equal, value);
            }

            throw NewError("Can't assign a non-variable value.", equal);
        }

        return expression;
    }


    private Expression AndOr()
    {
        Expression expression = Equality();

        while (Match(TokenType.And, TokenType.Or))
        {
            Token @operator = Previous();
            Expression right = Equality();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Equality()
    {
        Expression expression = Comparison();

        while (Match(TokenType.EqualEqual, TokenType.BangEqual))
        {
            Token @operator = Previous();
            Expression right = Comparison();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Comparison()
    {
        Expression expression = Term();

        while (Match(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual))
        {
            Token @operator = Previous();
            Expression right = Term();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Term()
    {
        Expression expression = Factor();

        while (Match(TokenType.Plus, TokenType.Minus))
        {
            Token @operator = Previous();
            Expression right = Factor();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Factor()
    {
        Expression expression = Unary();

        while (Match(TokenType.Asterisk, TokenType.Slash))
        {
            Token @operator = Previous();
            Expression right = Unary();
            expression = new BinaryExpression(expression, @operator, right);
        }

        return expression;
    }


    private Expression Unary()
    {
        if (Match(TokenType.Minus, TokenType.Not, TokenType.Typeof))
        {
            Token @operator = Previous();
            Expression right = Unary();
            return new UnaryExpression(@operator, right);
        }

        return Primary();
    }


    private Expression Primary()
    {
        if (Match(TokenType.String, TokenType.FloatNumber, TokenType.IntegerNumber, TokenType.UIntegerNumber,
                            TokenType.True, TokenType.False, TokenType.Null))
            return new LiteralExpression(Previous().Value);

        if (Match(TokenType.Identifier))
            return new IdentifierExpression(Previous());

        if (Match(TokenType.ParenLeft))
            return ParseGroupingExpression();

        throw NewError("Expression expected.", AtEnd() ? Previous() : Peek());
    }


    private GroupingExpression ParseGroupingExpression()
    {
        Token start = Previous();
        Expression expression = Expression();

        Expect(TokenType.ParenRight, "Unclosed parenthesis.", start);

        return new GroupingExpression(expression);
    }




    private void Synchronize()
    {
        Advance();

        while (!AtEnd())
        {
            switch (Peek().Type)
            {
                case TokenType.Print:
                case TokenType.Var:
                    return;
            }

            Advance();
        }
    }


    private Token Expect(TokenType token, string message, Token? reference = null)
    {
        if (Check(token))
            return Advance();

        throw NewError(message, reference ?? Peek());
    }


    private CanvazLangException NewError(string message, Token? token = null)
        => new(new(token ?? Peek()), message);


    private bool Match(params TokenType[] tokens)
    {
        foreach (TokenType token in tokens)
        {
            if (!Check(token))
                continue;

            Advance();
            return true;
        }

        return false;
    }

    private bool Check(TokenType tokenType)
    {
        if (AtEnd())
            return false;

        return Peek().Type == tokenType;
    }

    private bool AtEnd()
        => _current >= _tokens.Count;

    private bool AtBeginning()
        => _current <= 0;
    

    private Token Advance()
    {
        if (!AtEnd())
            _current++;
        
        return Previous();
    }

    private Token Retreat()
    {
        if (!AtBeginning())
            _current--;
        
        return Next();
    }

    private Token Peek()
        => _tokens[_current];

    private Token Next()
        => _tokens[_current + 1];

    private Token Previous()
        => _tokens[_current - 1];

}