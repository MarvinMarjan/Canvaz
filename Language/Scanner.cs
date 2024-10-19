using System;
using System.Collections.Generic;
using System.Globalization;
using Canvaz.Language.Exception;


namespace Canvaz.Language;


public class Scanner
{
    private string _source = "";
    private int _start, _end;
    private int _relativeStart, _relativeEnd;
    private int _line;

    private List<Token> _tokens = [];


    public List<Token> Scan(string source)
    {
        _tokens.Clear();
        _source = source;
        _start = _end = 0;
        _relativeStart = _relativeEnd = 0;
        _line = 1;

        while (!AtEnd())
        {
            _start = _end;
            _relativeStart = _relativeEnd;
            ScanToken();
        }

        return _tokens;
    }


    private void ScanToken()
    {
        char ch = Advance();

        switch (ch)
        {
        case ' ':
        case '\t':
        case '\r':
            break;

        case '\n':
            Newline();
            break;

        case '+': AddToken(TokenType.Plus); break;
        case '-': AddToken(TokenType.Minus); break;
        case '*': AddToken(TokenType.Asterisk); break;
        case '/': AddToken(TokenType.Slash); break;
        
        case '=': AddToken(TokenType.Equal); break;

        case '(': AddToken(TokenType.ParenLeft); break;
        case ')': AddToken(TokenType.ParenRight); break;
        case '[': AddToken(TokenType.BracketLeft); break;
        case ']': AddToken(TokenType.BracketRight); break;
        case '{': AddToken(TokenType.BraceLeft); break;
        case '}': AddToken(TokenType.BraceRight); break;

        case ':': AddToken(TokenType.Colon); break;
        case ',': AddToken(TokenType.Comma); break;

        case '"': String(); break;

        default:
            if (char.IsLetter(ch))
                Identifier();
            
            else if (char.IsDigit(ch))
                Number();

            else
                throw new CanvazLangException(new(TokenFromCurrent()), "Invalid token.");

            break;
        }
    }


    private void AddToken(TokenType tokenType, object? value = null)
        => _tokens.Add(TokenFromCurrent(tokenType, value));


    private Token TokenFromCurrent(TokenType tokenType = TokenType.Invalid, object? value = null)
        => new(CurrentSubstring(), _relativeStart, _relativeEnd, _line, tokenType, value);


    private void Newline()
    {
        _line++;
        _relativeStart = _relativeEnd = 0;
    }


    private void Identifier()
    {
        while (char.IsLetterOrDigit(Peek()))
            Advance();

        if (Token.Keywords.TryGetValue(CurrentSubstring(), out TokenType keyword))
            AddToken(keyword);
        else
            AddToken(TokenType.Identifier);
    }


    private void String()
    {
        while (Peek() != '"')
            Advance();
        
        Advance();

        string value = _source[(_start + 1) .. (_end - 1)];

        AddToken(TokenType.String, value);
    }


    private void Number()
    {
        bool hasDot = false;

        AdvanceUntilNonDigit();

        if (Peek() == '.')
        {
            Advance();
            AdvanceUntilNonDigit();

            hasDot = true;
        }

        TokenType tokenType = Token.NumberTypeFromSuffix(Peek(), hasDot);
        string text = CurrentSubstring();

        if (Token.IsNumberSuffix(Peek()))
            Advance();

        switch (tokenType)
        {
        case TokenType.FloatNumber:
            AddNumberToken<float>(text, tokenType);
            break;

        case TokenType.IntegerNumber:
            AddNumberToken<int>(text, tokenType);
            break;

        case TokenType.UIntegerNumber:
            AddNumberToken<uint>(text, tokenType);
            break;
        }
    }

    private void AddNumberToken<T>(string text, TokenType tokenType) where T : IParsable<T>
    {
        _ = T.TryParse(text, CultureInfo.InvariantCulture, out T? valueFloat);
        AddToken(tokenType, valueFloat);
    }

    private void AdvanceUntilNonDigit()
    {
        while (char.IsDigit(Peek()))
            Advance();
    }


    private string CurrentSubstring()
        => _source[_start .. _end];

    private bool AtEnd() => _end >= _source.Length;

    private char Advance()
    {
        if (!AtEnd())
        {
            _relativeEnd++;
            return _source[_end++];
        }
        
        return '\0';
    }

    private char Peek() => _source[_end];
}