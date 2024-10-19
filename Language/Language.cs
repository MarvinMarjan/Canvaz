using System;


namespace Canvaz.Language;


public static class CanvazLanguage
{
    public static string CurrentSource { get; private set; } = "";
    public static string[] CurrentSourceLines { get; private set; } = [];


    public static void Run(string source)
    {
        CurrentSource = source;
        CurrentSourceLines = CurrentSource.Split('\n');

        Scanner scanner = new();

        foreach (Token token in scanner.Scan(CurrentSource))
            Console.WriteLine(token.ToString());
    }
}