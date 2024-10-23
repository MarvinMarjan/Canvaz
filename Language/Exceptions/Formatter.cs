using System.Text;

using Specter.ANSI;
using Specter.Color;

using Canvaz.Language.Definitions;


namespace Canvaz.Language.Exceptions;


public static class ExceptionFormatter
{
    public static string GenerateHighlightArrowFromRange(TokenRange range, ColorObject arrowColor)
    {
        StringBuilder arrow = new(arrowColor.AsSequence());

        for (int i = 0; i < range.End.End; i++)
        {
            if (i == range.Start.Start)
                arrow.Append('^');
            else if (i > range.Start.Start)
                arrow.Append('~');
            else
                arrow.Append(' ');
        }

        arrow.Append(EscapeCodes.Reset);

        return arrow.ToString();
    }


    public static string HighlightTokenWithColor(TokenRange range, ColorObject color)
    {
        string line = CanvazLanguage.CurrentSourceLines[range.Start.Line - 1];
        line = line.Insert(range.End.End, ColorValue.Reset.AsSequence());
        line = line.Insert(range.Start.Start, color.AsSequence());

        return line;
    }
}