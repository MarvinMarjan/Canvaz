using System.Text;

using Specter.ANSI;
using Specter.Color;


namespace Canvaz.Language.Exception;


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
}