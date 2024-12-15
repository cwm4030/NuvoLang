namespace NuvoLang;

public static class Nuvo
{
    //public static bool HadError = false;

    public static void RunFile(string file)
    {
        var source = File.ReadAllText(file);
        Run(source);
    }

    public static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            var source = Console.ReadLine() ?? string.Empty;
            if (source == "quit") break;
            Run(source);
        }
    }

    private static void Run(string source)
    {
        var scanner = new Scanner(source);
        var tokens = scanner.ScanTokens();
        foreach (var token in tokens)
        {
            Console.WriteLine(token.Display);
        }
    }

    public static void Error(string message, int line)
    {
        Report(message, string.Empty, line);
    }

    private static void Report(string message, string where, int line)
    {
        Console.WriteLine($"[line {line}] Error {where}: {message}");
        //HadError = true;
    }
}
