using System;
using System.Collections.Generic;

using Canvaz.Language.Exceptions;


using Type = Canvaz.Language.Typing.Type;


namespace Canvaz.Language;


public class Scanner
{
    private string _source = "";
    private int _start, _end;
    private int _relativeStart, _relativeEnd;
    private int _line;

    private readonly List<Token> _tokens = [];


    public List<Token> Scan(string source)
    {
        _tokens.Clear();
        _source = source;
        _start = _end = 0;
        _relativeStart = _relativeEnd = 0;
        _line = 1;

        while (!AtEnd())
        {
            try
            {
                _start = _end;
                _relativeStart = _relativeEnd;
                ScanToken();
            }
            catch (Exception e)
            {
                CanvazLanguage.Error(e);
            }
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

        case '#':
            if (Match('<'))
                MultiLineComment();
            else
                SingleLineComment();

            break;

        case '+': AddToken(TokenType.Plus); break;
        case '-': AddToken(TokenType.Minus); break;
        case '*': AddToken(TokenType.Asterisk); break;
        case '/': AddToken(TokenType.Slash); break;
        
        case '=': AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal); break;
        case '!':
            if (Match('='))
                AddToken(TokenType.BangEqual);
            else
                throw NewError("'!' is invalid alone.");

            break;

        case '>': AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater); break;
        case '<': AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less); break;

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
                throw NewError("Invalid token.");

            break;
        }
    }


    private void AddToken(TokenType tokenType, Type? value = null)
        => _tokens.Add(TokenFromCurrent(tokenType, value));


    private Token TokenFromCurrent(TokenType tokenType = TokenType.Invalid, Type? value = null)
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
            AddToken(keyword, new(keyword switch {
                TokenType.True => true,
                TokenType.False => false,
                TokenType.Null => null,

                _ => null
            }, new(TokenFromCurrent())));
        else
            AddToken(TokenType.Identifier);
    }


    private void String()
    {
        Token stringStart = TokenFromCurrent();

        while (Peek() != '"')
        {
            Advance();
        
            if (AtEnd())
                throw NewError("Unclosed string.", stringStart);
            
            if (Peek() == '\n')
                Newline();
        }
        
        Advance();

        string value = _source[(_start + 1) .. (_end - 1)];

        AddToken(TokenType.String, new(value, new(stringStart, TokenFromCurrent())));
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

        // advance suffix 'f', 'i' or 'u', if present.
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
        _ = T.TryParse(text, null, out T? value);
        AddToken(tokenType, new(value, new(TokenFromCurrent())));
    }

    private void AdvanceUntilNonDigit()
    {
        while (char.IsDigit(Peek()))
            Advance();
    }


    private void SingleLineComment()
    {
        while (Peek() != '\n')
            Advance();
    }

    private void MultiLineComment()
    {
        Token commentStart = TokenFromCurrent();

        while (true)
        {
            if (NextIsEnd())
                throw new CanvazLangException(new(commentStart), "Unclosed comment.");

            if (Match('>') && Match('#'))
                break;

            if (Advance() == '\n')
                Newline();
        }
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

    private bool NextIsEnd() => _end + 1 >= _source.Length;

    private bool Match(char ch)
    {
        if (AtEnd() || Peek() != ch)
            return false;

        Advance();
        return true;
    }

    private char Peek() => AtEnd() ? '\0' : _source[_end];


    private CanvazLangException NewError(string message, Token? token = null)
        => new(new(token ?? TokenFromCurrent()), message);
}