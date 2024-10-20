using System;
using System.Text;

using Specter.Color;


namespace Canvaz.Language.Exceptions;


[Serializable]
public class CanvazLangException : Exception
{
    public TokenRange? TokenRange { get; init; }


    public CanvazLangException()
    { }

    public CanvazLangException(TokenRange? tokenRange, string message)
        : base(message)
    {
        TokenRange = tokenRange;
    }

    public CanvazLangException(TokenRange? tokenRange, string message, Exception inner)
        : base(message, inner)
    {
        TokenRange = tokenRange;
    }


    public override string ToString()
    {
        if (TokenRange is not TokenRange validRange)
            return Message;

        StringBuilder builder = new();

        string arrow = ExceptionFormatter.GenerateHighlightArrowFromRange(validRange, ColorObject.FromColor16(Color16.FGBRed));
        string line = ExceptionFormatter.HighlightTokenWithColor(validRange, ColorValue.FGRed);

        string lineNumber = $"{validRange.Start.Line}. ";
        arrow = new string(' ', lineNumber.Length) + arrow;
    
        builder.AppendLine(Message);
        builder.AppendLine($"{lineNumber}{line}\n{arrow}");

        return builder.ToString();
    }
}