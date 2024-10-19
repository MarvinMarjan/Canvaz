using System;
using System.Text;

using Specter.Color;


namespace Canvaz.Language.Exception;


[Serializable]
public class CanvazLangException : System.Exception
{
    public TokenRange TokenRange { get; init; }


    public CanvazLangException()
    { }

    public CanvazLangException(TokenRange tokenRange, string message)
        : base(message)
    {
        TokenRange = tokenRange;
    }

    public CanvazLangException(TokenRange tokenRange, string message, System.Exception inner)
        : base(message, inner)
    {
        TokenRange = tokenRange;
    }


    public override string ToString()
    {
        StringBuilder builder = new();

        string arrow = ExceptionFormatter.GenerateHighlightArrowFromRange(TokenRange, ColorObject.FromColor16(Color16.FGRed));
        string line = CanvazLanguage.CurrentSourceLines[TokenRange.Start.Line - 1];

        string lineNumber = $"{TokenRange.Start.Line}. ";
        arrow = new string(' ', lineNumber.Length) + arrow;
    
        builder.AppendLine(Message);
        builder.AppendLine($"{lineNumber}{line}\n{arrow}");

        return builder.ToString();
    }
}