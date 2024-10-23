using System;
using System.Collections.Generic;


using Type = Canvaz.Language.Typing.Type;


namespace Canvaz.Language.Definitions;


/// <summary>
/// All types of tokens.
/// </summary>
public enum TokenType
{
    Invalid,

    Hash,

    Plus, Minus, Asterisk, Slash,
    Equal, EqualEqual, BangEqual,

    Greater, GreaterEqual, Less, LessEqual,

    ParenLeft, ParenRight, BracketLeft, BracketRight, BraceLeft, BraceRight,

    Colon, Comma, Quote,

    String, True, False, Null,
    FloatNumber, IntegerNumber, UIntegerNumber,
    Identifier,

    Not, And, Or,
    Typeof,
    Print, Var,
    Structure,
    If, Else, While,
}


public readonly struct TokenRange
{
    public Token Start { get; init; }
    public Token End { get; init; }


    public TokenRange(Token start, Token end)
    {
        Start = start;
        End = end;

        if (Start.Line != End.Line)
            throw new ArgumentException("A TokenRange can't store tokens in different lines.");
    }


    public TokenRange(Token token)
        : this(token, token)
    {}
}


public readonly struct Token(string lexeme, int start, int end, int line, TokenType tokenType, Type? value = null)
{
    public static Dictionary<string, TokenType> Keywords { get; } = new([
        new("not", TokenType.Not),
        new("and", TokenType.And),
        new("or", TokenType.Or),
        new("typeof", TokenType.Typeof),
        new("true", TokenType.True),
        new("false", TokenType.False),
        new("null", TokenType.Null),
        new("print", TokenType.Print),
        new("var", TokenType.Var),
        new("structure", TokenType.Structure),
        new("if", TokenType.If),
        new("else", TokenType.Else),
        new("while", TokenType.While),
    ]);


    public static bool IsKeyword(string identifier, out TokenType? keyword, out bool isKeywordValue)
    {
        bool isKeyword;
        keyword = null;

        if (isKeyword = Keywords.TryGetValue(identifier, out TokenType value))
            keyword = value;
        
        isKeywordValue = identifier is "true" or "false" or "null";
    
        return isKeyword;
    }


    public string Lexeme { get; init; } = lexeme;
    public int Start { get; init; } = start;
    public int End { get; init; } = end;
    public int Line { get; init; } = line;
    public TokenType Type { get; init; } = tokenType;
    public Type Value { get; init; } = value ?? new(null);


    public override string ToString()
        => $"\"{Lexeme}\" -> {Type}";


    public static bool IsNumberSuffix(char ch)
        => ch is 'f' or 'i' or 'u';

    public static TokenType NumberTypeFromSuffix(char suffix, bool hasDot = false) => suffix switch {
        'f' => TokenType.FloatNumber,
        'i' => TokenType.IntegerNumber,
        'u' => TokenType.UIntegerNumber,

        _ => hasDot ? TokenType.FloatNumber : TokenType.IntegerNumber
    };
}