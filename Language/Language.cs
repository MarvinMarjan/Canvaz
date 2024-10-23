using System;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;

using Specter.String;

using Canvaz.Language.Exceptions;
using Canvaz.Language.Definitions;
using Canvaz.Language.Tools;


namespace Canvaz.Language;


public static class CanvazLanguage
{
    public static string CurrentSource { get; private set; } = "";
    public static string[] CurrentSourceLines { get; private set; } = [];

    /// <summary>
    /// The token reference of the current operation the interpreter is executing.
    /// </summary>
    public static Token? CurrentRuntimeTokenReference { get; set; }

    public static bool HasError { get; private set; }


    private static void Setup()
    {
        CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        Thread.CurrentThread.CurrentCulture = customCulture;
    }


    public static void Run(string source)
    {
        Setup();

        CurrentSource = source;
        CurrentSourceLines = CurrentSource.Split('\n');

        try
        {
            Scanner scanner = new();
            List<Token> tokens = scanner.Scan(CurrentSource);

            if (HasError || tokens.Count == 0)
                return;

            Parser parser = new();
            List<Statement> statements = parser.Parse(tokens);
        
            if (HasError)
                return;

            new Interpreter().Interpret(statements);
        }
        catch (Exception e)
        {
            Error(e);
        }
    }


    public static void Error(Exception e)
    {
        string msg = e is CanvazLangException ? e.ToString() : e.Message;

        Console.WriteLine($"{"Error:".FGBRed()} {msg}");
        HasError = true;
    }
}